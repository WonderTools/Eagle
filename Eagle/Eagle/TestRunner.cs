using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Engine;

namespace Eagle
{
    public class TestRunner
    {
        private readonly IMyLogger _logger;
        private readonly Dictionary<string, (TestPackage TestPackage, string FullName)> _idToSchedulingParametersMap;
        private readonly ITestQueue _testQueue;
        private ScheduledTestInternal _runningTest;
        private object _lockable = new object();

        public TestRunner(IMyLogger logger, Dictionary<string, (TestPackage TestPackage, string FullName)> idToSchedulingParametersMap, ITestQueue testQueue)
        {
            _logger = logger;
            _idToSchedulingParametersMap = idToSchedulingParametersMap;
            _testQueue = testQueue;
        }

        public void Process()
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

            var t = Task.Run(async () =>
            {
                var schedulingParameters = _idToSchedulingParametersMap[_runningTest.Id];
                RunTestCase(schedulingParameters.TestPackage, schedulingParameters.FullName);
                lock (_lockable)
                {
                    _runningTest = null;
                }
            });
        }

        private void RunTestCase(TestPackage testPackage, string name)
        {
            using (var engine = TestEngineActivator.CreateInstance())
            {
                using (var runner = engine.GetRunner(testPackage))
                {
                    var filterService = engine.Services.GetService<ITestFilterService>();
                    var builder = filterService.GetTestFilterBuilder();
                    //builder.AddTest("Feature.Infrastructure.TestClass.TestMethod");
                    //builder.AddTest("Feature.Infrastructure.TestMyClass.MyTest");
                    //builder.AddTest("Feature.Infrastructure.TestMyClass.MyTest(4)");
                    builder.AddTest(name);
                    var filter = builder.GetFilter();
                    var result = runner.Run(new TestEventListener(), filter).ToJson();
                    //runner.RunAsync(new TestEventListener(), filter).Result.ToJson();
                    _logger.Log("***start***");
                    _logger.Log(result);
                    _logger.Log("***end***");
                }
            }
        }
    }
}