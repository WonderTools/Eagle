using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using Eagle.NUnitResult;
using NUnit.Engine;

namespace Eagle
{
    public class TestRunner
    {
        private readonly Dictionary<string, (TestPackage TestPackage, string FullName)> _idToSchedulingParametersMap;
        private readonly ITestQueue _testQueue;
        private readonly IEagleEventListener _eventListener;
        private ScheduledTestInternal _runningTest;
        private object _lockable = new object();

        public TestRunner(Dictionary<string, (TestPackage TestPackage, string FullName)> idToSchedulingParametersMap, 
            ITestQueue testQueue, IEagleEventListener eventListener)
        {
            _idToSchedulingParametersMap = idToSchedulingParametersMap;
            _testQueue = testQueue;
            _eventListener = eventListener;
        }

        public async Task Process()
        {
            
            lock (_lockable)
            {
                if (_runningTest != null) return;
            }

            lock (_lockable)
            {
                _runningTest = _testQueue.RemoveTopOfQueue();
                if (_runningTest == null) return;
            }

            await Task.Run(async () =>
            {
                await RunTestCase(_runningTest.Id);
                lock (_lockable)
                {
                    _runningTest = null;
                }
            });
        }

        public async Task ExecuteOne()
        {
            var top = _testQueue.RemoveTopOfQueue();
            if(top == null) return;
            await RunTestCase(top.Id);
        }

        private async Task RunTestCase(string id)
        {
            var schedulingParameters = _idToSchedulingParametersMap[id];
            await RunTestCase(schedulingParameters.TestPackage, schedulingParameters.FullName);
        }

        private async Task RunTestCase(TestPackage testPackage, string name)
        {
            using (var engine = TestEngineActivator.CreateInstance())
            {
                using (var runner = engine.GetRunner(testPackage))
                {
                    //TBD: Check if the scheduling can be done with internal id
                    var filterService = engine.Services.GetService<ITestFilterService>();
                    var builder = filterService.GetTestFilterBuilder();
                    builder.AddTest(name);
                    var filter = builder.GetFilter();
                    var x =   runner.Run(new TestEventListener(), filter);
                    await HandleResults(x.ToJson());
                }
            }
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

        private async Task HandleResults(string json)
        {
            Console.WriteLine("Handling result" + json);
            var jsonParser = new NUnitJsonParser();
            var runTestSuite= jsonParser.GetTestSuiteFromResultJson(json);
            var testCases= GetTestCases(runTestSuite);
            foreach (var testCase in testCases)
            {
                await _eventListener.TestCompleted(testCase.FullName.GetIdFromFullName(),
                    testCase.Result, testCase.StartTime, testCase.EndTime, (int)testCase.Duration);
            }
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