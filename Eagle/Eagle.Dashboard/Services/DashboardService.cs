using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eagle.Dashboard.Models;

namespace Eagle.Dashboard.Services
{
    public class DashboardService
    {
        private readonly IDataStore _dataStore;
        private readonly ITestScheduler _testScheduler;

        public DashboardService(IDataStore dataStore, ITestScheduler testScheduler)
        {
            _dataStore = dataStore;
            _testScheduler = testScheduler;
        }

        public async Task CreateNode(NodeCreationParameters creationParameters)
        {
            //TODO : Validate model
            await _dataStore.CreateNode(creationParameters);

            await ScheduleTestAndAddRequest(creationParameters.NodeName, creationParameters.Uri, string.Empty);
        }

        private async Task ScheduleTestAndAddRequest(string nodeName, string uri, string testId)
        {
            var requestTime = DateTime.Now;
            var requestId = GenerateResultId(requestTime);
            //TODO : The Uri is Currently hardcoded, but this needs to be injected. It also needs to be configured
            //TODO : This needs to be injected as we could switch to messaging using RabbitMQ
            var isRequestSuccessful = await ScheduleTest(nodeName, uri, testId,
                "https://localhost:6501/api/results", requestId);
            await _dataStore.AddRequest(requestId, nodeName, testId, requestTime, isRequestSuccessful);
        }

        private async Task<bool> ScheduleTest(string nodeName, string uri, string testId, string callBackUri, string requestId)
        {
            try
            {
                await _testScheduler.Schedule(nodeName, uri, testId, requestId, callBackUri);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string GenerateResultId(DateTime requestTime)
        {
            return requestTime.ToString("yyyy-MM-dd-HH:mm:ss.fff") + new Guid();
        }

        public async Task AddResults(MyResult result)
        {
            AdjustId(result.NodeName, result.TestSuites);
            AdjustId(result.NodeName, result.TestResults);

            await _dataStore.AddDiscoveredTests(result.NodeName, result.TestSuites);
            await _dataStore.AddTestResults(result.NodeName, result.TestResults);
        }

        private void AdjustId(string nodeName, List<TestResult> testResults)
        {
            foreach (var testResult in testResults)
            {
                testResult.Id = GetAdjustedId(nodeName, testResult.Id);
            }
        }

        private void AdjustId(string nodeName, List<TestSuite> resultTestSuites)
        {
            foreach (var resultTestSuite in resultTestSuites)
            {
                resultTestSuite.Id = GetAdjustedId(nodeName, resultTestSuite.Id);
                AdjustId(nodeName, resultTestSuite.TestCases);
                AdjustId(nodeName, resultTestSuite.TestSuites);
            }
        }

        private static void AdjustId(string nodeName, List<TestCase> testCases)
        {
            foreach (var testCase in testCases)
            {
                testCase.Id = GetAdjustedId(nodeName, testCase.Id);
            }
        }

        private static string GetAdjustedId(string nodeName, string baseId)
        {
            return nodeName + GetSeparator() + baseId;
        }

        private static string GetSeparator()
        {
            return "-->--";
        }

        public async Task<List<TestSuiteModel>> GetResults()
        {
            var dictionary = await _dataStore.GetLatestTestSuites();
            var testSuites = dictionary.Select(x => GroupAsNodeTestSuite(x.Key, x.Value));
            var testSuiteModels = ConvertToListOfListOfTestSuiteModel(testSuites);
            
            List<TestCaseModel> testCases = GetTestCases(testSuiteModels);
            var testCaseDictionary = testCases.ToDictionary(x => x.Id, x => x);


            var results = await _dataStore.GetLatestTestResults();
            foreach (var testResult in results)
            {
                if (!testCaseDictionary.ContainsKey(testResult.Id)) continue;
                var x = testCaseDictionary[testResult.Id];
                x.Result = testResult.Result;
                x.StartingTime = testResult.StartTime;
                x.FinishingTime = testResult.EndTime;
                x.DurationInMs = testResult.DurationInMs;
            }

            return testSuiteModels;
        }



        private static List<TestSuiteModel> ConvertToListOfListOfTestSuiteModel(IEnumerable<TestSuite> testSuites)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestSuite, TestSuiteModel>();
                cfg.CreateMap<TestCase, TestCaseModel>();
            });
            var testSuiteModels = config.CreateMapper().Map<List<TestSuiteModel>>(testSuites);
            return testSuiteModels;
        }

        private static TestSuite GroupAsNodeTestSuite(string nodeName, List<TestSuite> testSuites)
        {
            return new TestSuite()
            {
                Id = nodeName + GetSeparator(),
                FullName = nodeName,
                Name = nodeName,
                TestSuites = testSuites,
            };
        }

        private List<TestCaseModel> GetTestCases(List<TestSuiteModel> testSuites)
        {
            var testCases = new List<TestCaseModel>();
            foreach (var testSuiteModel in testSuites)
            {
                testCases.AddRange(testSuiteModel.TestCases);
                testCases.AddRange(GetTestCases(testSuiteModel.TestSuites));
            }
            return testCases;
        }

        public async Task ScheduleTests(string id)
        {
            var strs = id.Split(GetSeparator());
            if (strs.Length != 2) throw new Exception();
            var nodeName = strs[0];
            var testId = strs[1];
            var uri = await _dataStore.GetUri(nodeName);
            await ScheduleTestAndAddRequest(nodeName, uri, testId);
        }
    }
}
