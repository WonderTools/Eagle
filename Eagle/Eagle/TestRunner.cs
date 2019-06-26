using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using WonderTools.Eagle.Contract;
using Eagle.NUnitResult;
using NUnit.Engine;

namespace Eagle
{
    public class TestRunner
    {
        public async Task<List<TestResult>> RunTestCase(TestPackage testPackage, string name = "")
        {
            using (var engine = TestEngineActivator.CreateInstance())
            {
                using (var runner = engine.GetRunner(testPackage))
                {
                    await Task.Delay(1); //Just to make sure we can have awaitable
                    var filter = TestFilter(engine, name);
                    var x = runner.Run(new TestEventListener(), filter);
                    return GetTestResults(x.ToJson());
                }
            }
        }

        private static TestFilter TestFilter(ITestEngine engine, string name = "")
        {
            var filterService = engine.Services.GetService<ITestFilterService>();
            var builder = filterService.GetTestFilterBuilder();
            if (!string.IsNullOrWhiteSpace(name)) builder.AddTest(name);
            var filter = builder.GetFilter();
            return filter;
        }

        private List<TestResult> GetTestResults(string json)
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