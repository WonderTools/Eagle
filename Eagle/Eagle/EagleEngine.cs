using NUnit.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Eagle
{
    public class EagleEngine
    {
        private List<TestSuite> _testSuites;
        private Dictionary<string,(TestPackage TestPackage, string FullName)> _idToSchedulingParametersMap;

        private readonly IMyLogger _logger;
        ITestQueue _testQueue = new ThreadSafeQueue();
        private object _lockable = new object();
        private ScheduledTest _runningTest;
        public EagleEngine(IMyLogger logger)
        {       
            _logger = logger;
        }

        public async Task Process()
        {
            ProcessInternal();
        }

        private void ProcessInternal()
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


        public string ScheduleTest(string id)
        {
            if (!_idToSchedulingParametersMap.ContainsKey(id)) throw new Exception("The Id is not found");
            return _testQueue.AddToQueue(id);
        }

        public List<ScheduledTest> GetScheduledFeatures()
        {
            return _testQueue.GetQueueElements();
        }

        public List<TestSuite> GetDiscoveredTestSuites()
        {
            return _testSuites;
        }

        public void Initialize(params TestAssemblyLocationHolder[] testAssembliesLocationHolder)
        {
            Initializer initializer = new Initializer();
            var packageToSuiteMap = initializer.GetTestPackageToTestSuiteMap(testAssembliesLocationHolder);
            _testSuites = packageToSuiteMap.Values.ToList();
            _idToSchedulingParametersMap = initializer.GetIdToSchedulingParametersMap(packageToSuiteMap);
        }
    }
}   