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

        ITestQueue _testQueue = new ThreadSafeQueue();
        
        private readonly TestRunner _testRunner;

        public EagleEngine(IMyLogger logger)
        {
            _testRunner = new TestRunner(logger, _idToSchedulingParametersMap, _testQueue);
        }

        public async Task Process()
        {
            _testRunner.Process();
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