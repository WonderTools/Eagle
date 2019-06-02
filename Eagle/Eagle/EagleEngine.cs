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
        private Dictionary<string,TestPackage> _idToTestPackageMap;


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
                RunTestCase(_idToTestPackageMap[_runningTest.Id], _runningTest.Id);
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
                    var result = runner.Run(new TestEventListener(), filter);
                    _logger.Log("");
                    _logger.Log(result.InnerText);
                    _logger.Log("");
                }
            }
        }


        public string ScheduleTest(string id)
        {
            if (!_idToTestPackageMap.ContainsKey(id)) throw new Exception("The Id is not found");
            lock (_testQueue)
            {
                return _testQueue.Add(id, id);
            }
        }

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

    public static class XmlExtensions
    {
        public static string ToJson(this XmlNode xmlNode)
        {
            var myData = xmlNode.OuterXml;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(myData);
            return JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
        }
    }
}   