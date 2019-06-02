using AutoMapper;
using Eagle.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Web.Service
{
    public class TestTreeService
    {
        private readonly EagleEngine _eagleEngine;
        private readonly IResultRepository _resultRepository;

        public TestTreeService(EagleEngine eagleEngine, IResultRepository resultRepository)
        {
            _eagleEngine = eagleEngine;
            _resultRepository = resultRepository;
        }

        public async Task<List<TestSuiteModel>> GetTestTree()
        {
            var testSuites = _eagleEngine.GetDiscoveredTestSuites();
            var testSuiteModels = Convert(testSuites);
            testSuiteModels = await FillResultDetails(testSuiteModels);
            return testSuiteModels;
        }

        private async Task<List<TestSuiteModel>> FillResultDetails(List<TestSuiteModel> testSuites)
        {
            var t = await _resultRepository.GetAllRecentTestResults();
            var testResults = t.ToDictionary(x => x.Id, x => x);
            var testCaseModels = GetTestCases(testSuites);
            foreach (var testCaseModel in testCaseModels)
            {
                if (testResults.ContainsKey(testCaseModel.Id))
                {
                    var testResult = testResults[testCaseModel.Id];
                    testCaseModel.Result = testResult.Result;
                    testCaseModel.StartingTime = testResult.StartingTime;
                    testCaseModel.FinishingTime = testResult.FinishingTime;
                    testCaseModel.DurationInMs = testResult.DurationInMs;
                }
            }
            return testSuites;
        }


        private List<TestCaseModel> GetTestCases(List<TestSuiteModel> testSuites)
        {
            var testCaseDoubleList = testSuites.Select(GetTestCases);
            return testCaseDoubleList.SelectMany(l => l).ToList();
        }

        private List<TestCaseModel> GetTestCases(TestSuiteModel testSuite)
        {
            var testCases = GetTestCases(testSuite.TestSuites);
            testCases.AddRange(testSuite.TestCases);
            return testCases;
        }

        private List<TestSuiteModel> Convert(List<TestSuite> suites)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestSuite, TestSuiteModel>();
                cfg.CreateMap<TestCase, TestCaseModel>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<List<TestSuiteModel>>(suites);
        }
    }
}
