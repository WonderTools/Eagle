using System;
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
        private readonly IEagleEventListener _eventListener;
        private ScheduledTestInternal _runningTest;
        private object _lockable = new object();

        public TestRunner(IMyLogger logger, Dictionary<string, (TestPackage TestPackage, string FullName)> idToSchedulingParametersMap, 
            ITestQueue testQueue, IEagleEventListener eventListener)
        {
            _logger = logger;
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

        private async Task HandleResults(string json)
        {
            Console.WriteLine("Handling result" + json);

            ////TBD: This has to be implemented
            await _eventListener.TestCompleted("idFeature.Infrastructure.EagleFeature.AddTwoNumbers", "pass", DateTime.Now,
                DateTime.Now ,10 );

        }
    }
}