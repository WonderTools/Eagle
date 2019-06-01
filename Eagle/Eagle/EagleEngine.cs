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
using Formatting = Newtonsoft.Json.Formatting;

namespace Eagle
{
    public class EagleEngine
    {
        private readonly IMyLogger _logger;
        TestQueue _testQueue = new TestQueue();
        private object _lockable = new object();
        private ScheduledFeature _runningTest;
        private List<TestPackage> _testPackages;


        public EagleEngine(IMyLogger logger)
        {
            _logger = logger;
        }

        public int Value { get; set; }

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

        public List<NameAndId> GetFeatureNames()
        {
            return _testPackages.SelectMany(GetFeatureNames).ToList();
        }
        
        private static string ToJson(XmlNode exploredNodes)
        {
            var myData = exploredNodes.OuterXml;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(myData);
            return JsonConvert.SerializeXmlNode(doc, Formatting.Indented);
        }

        private List<NameAndId> GetFeatureNames(TestPackage testPackage)
        {
            var names = GetTestCaseNames(testPackage);
            return names.Select(n => new NameAndId()
            {
                Name = n,
                //TBD: UrlEncode seems to happening wrong for "Abc( xyz )"
                Id = WebUtility.UrlEncode(n),
            }).ToList();
        }

        private List<string> GetTestCaseNames(TestPackage testPackage)
        {
            RunTestCase(testPackage);

            using (var engine = TestEngineActivator.CreateInstance())
            {
                using (var runner = engine.GetRunner(testPackage))
                {
                    var xml = runner.Explore(TestFilter.Empty);
                    var json = ToJson(xml);
                    return ParseNames(json);
                }
            }
        }


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

        private static List<string> ParseNames(string json)
        {
            //TBD: Remove the hard code
            return new List<string>()
            {
                "Feature.Infrastructure.EagleFeature.AddTwoNumbers",
                "Feature.Infrastructure.EagleFeature.SubtractTwoNumbers",
                "Feature.Infrastructure.TestClass",
                "Feature.Infrastructure.TestClass.TestMethod",
                "Abc( xyz )"
            };
        }

        private TestPackage GetTestPackage(Assembly assembly)
        {
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.GetDirectoryName(currentDirectory);

            var assemblyName = path +"\\"+ assembly.GetName().Name + ".dll";
            TestPackage package = new TestPackage(assemblyName);
            
            return package;
        }

        public string ScheduleFeature(string id)
        {
            var namesAndId = GetFeatureNames().FirstOrDefault(x => x.Id == id);
            if(namesAndId == null) throw new Exception("The Id is not found");
            lock (_testQueue)
            {
                return _testQueue.Add(id, namesAndId.Name);
            }
        }

        public List<ScheduledFeature> GetScheduledFeatures()
        {
            lock (_testQueue)
            {
                return _testQueue.GetElements();
            }
        }

        public void Initialize(EagleSeed eagleSeed)
        {
            _testPackages = eagleSeed.TestPackages;
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