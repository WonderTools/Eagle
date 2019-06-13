﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eagle.Dashboard.Models;
using Eagle.NUnitDiscovery;
using Microsoft.AspNetCore.Hosting.Server.Features;

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
            var requestTime = DateTime.Now;
            var requestId = GenerateResultId(requestTime);
            //TODO : The Uri is Currently hardcoded, but this needs to be injected. It also needs to be configured
            //TODO : This needs to be injected as we could switch to messaging using RabbitMQ
            var isRequestSuccessful = await ScheduleTest(creationParameters, requestId, "https://localhost:6501/api/results");
            await _dataStore.AddRequest(requestId, creationParameters.NodeName, string.Empty, requestTime, isRequestSuccessful);
        }

        private async Task<bool> ScheduleTest(NodeCreationParameters creationParameters, string requestId, string callBackUri)
        {
            try
            {
                await _testScheduler.Schedule(creationParameters.NodeName, creationParameters.Uri, string.Empty, requestId, callBackUri);
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
            return nodeName + "-->--" + baseId;
        }

        public async Task<List<TestSuiteModel>> GetResults()
        {
            var listOfListOfTestSuites = await _dataStore.GetLatestTestSuites();
            //var results = await _dataStore.GetLatestTestResults();

            var testSuites = listOfListOfTestSuites.SelectMany(x => x).ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestSuite, TestSuiteModel>();
                cfg.CreateMap<TestCase, TestCaseModel>();
            });

            var testSuiteModels =  config.CreateMapper().Map<List<TestSuiteModel>>(testSuites);


            List<TestCaseModel> testCases = GetTestCases(testSuiteModels);
            var testCaseDictionary = testCases.ToDictionary(x => x.Id, x => x);
            return testSuiteModels;
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
    }
}
