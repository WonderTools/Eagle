using NUnit.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Eagle
{
    public class EagleEngine
    {
        private readonly IMyLogger _logger;
        private readonly ITestQueue _testQueue;
        private TestRunner _testRunner;
        private List<TestSuite> _testSuites;
        private Dictionary<string,(TestPackage TestPackage, string FullName)> _idToSchedulingParametersMap;
        
        public EagleEngine(IMyLogger logger)
        {
            _testQueue = new ThreadSafeQueue();
            _logger = logger;
        }

        public async Task Process()
        {
            await _testRunner.Process();
        }

        public string ScheduleTest(string id)
        {
            if (!_idToSchedulingParametersMap.ContainsKey(id)) throw new Exception("The Id is not found");
            return _testQueue.AddToQueue(id);
        }

        public List<ScheduledTest> GetScheduledFeatures()
        {
            var queueElements = _testQueue.GetQueueElements();
            return queueElements.Select(x => new ScheduledTest()
            {
                //TBD Refactor to use auto mapper
                FullName = _idToSchedulingParametersMap[x.Id].FullName,
                Id = x.Id,
                SerialNumber = x.SerialNumber,
            }).ToList();
        }

        public List<TestSuite> GetDiscoveredTestSuites()
        {
            return _testSuites;
        }

        public void Initialize(IEagleEventListener eventListener, params TestAssemblyLocationHolder[] testAssembliesLocationHolder)
        {
            Initializer initializer = new Initializer();
            var packageToSuiteMap = initializer.GetTestPackageToTestSuiteMap(testAssembliesLocationHolder);
            _testSuites = packageToSuiteMap.Values.ToList();
            _idToSchedulingParametersMap = initializer.GetIdToSchedulingParametersMap(packageToSuiteMap);
            _testRunner = new TestRunner(_logger, _idToSchedulingParametersMap, _testQueue, eventListener);
        }
    }
}   