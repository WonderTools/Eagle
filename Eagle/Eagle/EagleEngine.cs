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
using System.Xml;
using Newtonsoft.Json;

namespace Eagle
{
    public class EagleEngine
    {
        private List<TestPackage> _testPackages;
        private List<TestSuite> _testSuites;
        private Dictionary<string, TestPackage> _idToTestPackageMap;


        private readonly IMyLogger _logger;
        TestQueue _testQueue = new TestQueue();
        private object _lockable = new object();
        private ScheduledFeature _runningTest;
        


        public EagleEngine(IMyLogger logger)
        {
            _logger = logger;
        }

        public async Task Process()
        {
            lock (_lockable)
            {
                if (_runningTest != null) return;
            }

            lock (_lockable)
            {
                lock (_testQueue)
                {
                    _runningTest = _testQueue.RemoveTopQueueElement();
                    if (_runningTest == null) return;
                }
            }

            var t = Task.Run(async () =>
            {
                _logger.Log($"-->{DateTime.Now.TimeOfDay}-{_runningTest.Id}--{_runningTest.Name}--{_runningTest.TestId}");
                await Task.Delay(15000);
                lock (_lockable)
                {
                    _runningTest = null;
                }
            });
        }

        //public List<NameAndId> GetFeatureNames()
        //{
        //    return _testPackages.SelectMany(GetFeatureNames).ToList();
        //}
        
        //private List<NameAndId> GetFeatureNames(TestPackage testPackage)
        //{
        //    var names = GetTestCaseNames(testPackage);
        //    return names.Select(n => new NameAndId()
        //    {
        //        Name = n,
        //        //TBD: UrlEncode seems to happening wrong for "Abc( xyz )"
        //        Id = WebUtility.UrlEncode(n),
        //    }).ToList();
        //}

        private void RunTestCase(TestPackage testPackage)
        {
            using (var engine = TestEngineActivator.CreateInstance())
            {
                using (var runner = engine.GetRunner(testPackage))
                {
                    var filterService = engine.Services.GetService<ITestFilterService>();
                    var builder = filterService.GetTestFilterBuilder();
                    //builder.AddTest("Feature.Infrastructure.TestClass.TestMethod");
                    builder.AddTest("Feature.Infrastructure.TestMyClass.MyTest");
                    //builder.AddTest("Feature.Infrastructure.TestMyClass.MyTest(4)");
                    var filter = builder.GetFilter();
                    var result = runner.Run(new TestEventListener(), filter);
                }
            }
        }


        //public string ScheduleFeature(string id)
        //{
        //    var namesAndId = GetFeatureNames().FirstOrDefault(x => x.Id == id);
        //    if(!_idToTestPackageMap.ContainsKey(id)) throw new Exception("The Id is not found");
        //    lock (_testQueue)
        //    {
        //        return _testQueue.Add(id, "some name");
        //    }
        //}

        public List<ScheduledFeature> GetScheduledFeatures()
        {
            lock (_testQueue)
            {
                return _testQueue.GetElements();
            }
        }

        public List<TestSuite> GetDiscoveredTestSuites()
        {
            return _testSuites;
        }

        public void Initialize(params TestAssemblyLocationHolder[] testAssembliesLocationHolder)
        {
            Initializer initializer = new Initializer();
            var initializationParameters = initializer.GetInitializationParameters(testAssembliesLocationHolder);
            _testPackages = initializationParameters.TestPackages;
            _testSuites = initializationParameters.TestSuites;
            _idToTestPackageMap = initializationParameters.IdToTestPackageMap;
        }
    }

    public class TestEventListener : ITestEventListener
    {
        public void OnTestEvent(string report)
        {
            Console.WriteLine(report);    
        }
    }
}   