using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using Eagle.Contract;
using Eagle.NUnitResult;
using NUnit.Engine;

namespace Eagle
{
    public class TestRunner
    {
        private readonly Dictionary<string, (TestPackage TestPackage, string FullName)> _idToSchedulingParametersMap;
        private ScheduledTestInternal _runningTest;

        public TestRunner(Dictionary<string, (TestPackage TestPackage, string FullName)> idToSchedulingParametersMap)
        {
            _idToSchedulingParametersMap = idToSchedulingParametersMap;
        }

        public async Task<List<TestResult>> RunTestCaseNew(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var schedulingParameters = _idToSchedulingParametersMap[id];
                return await RunTestCaseNew(schedulingParameters.TestPackage, schedulingParameters.FullName);
            }
            else
            {
                List<TestResult> results = new List<TestResult>();
                var packages = _idToSchedulingParametersMap.Select(x => x.Value.TestPackage).Distinct();

                foreach (var package in packages)
                {
                    var rs = await RunTestCaseNew(package, "");
                    results.AddRange(rs);
                }
                return results;
            }
        }

        private async Task<List<TestResult>> RunTestCaseNew(TestPackage testPackage, string name)
        {
            using (var engine = TestEngineActivator.CreateInstance())
            {
                using (var runner = engine.GetRunner(testPackage))
                {
                    //TBD: Check if the scheduling can be done with internal id
                    var filterService = engine.Services.GetService<ITestFilterService>();
                    var builder = filterService.GetTestFilterBuilder();
                    if(!string.IsNullOrWhiteSpace(name)) builder.AddTest(name);
                    var filter = builder.GetFilter();
                    var x = runner.Run(new TestEventListener(), filter);
                    return await HandleResultsNew(x.ToJson());
                }
            }
        }

        private async Task<List<TestResult>> HandleResultsNew(string json)
        {
            var jsonParser = new NUnitJsonParser();
            var runTestSuite = jsonParser.GetTestSuiteFromResultJson(json);
            var testCases = GetTestCases(runTestSuite);

            return testCases.Select(x => new TestResult()
            {
                Id = x.FullName.GetIdFromFullName(),
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                DurationInMs = x.Duration * 1000,
                Result = x.Result,

            }).ToList();
        }

        private List<ResultTestCase> GetTestCases(ResultTestSuite testSuite)
        {
            var result = new List<ResultTestCase>();
            result.AddRange(testSuite.TestCases);
            foreach (var tSuite in testSuite.TestSuites)
            {
                result.AddRange(GetTestCases(tSuite));
            }
            return result;
        }
    }
}