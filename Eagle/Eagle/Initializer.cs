using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using NUnit.Engine;
using Formatting = System.Xml.Formatting;

namespace Eagle
{
    public class Initializer
    {
        private TestSuite GetTestSuite(TestPackage testPackage)
        {
            using (var engine = TestEngineActivator.CreateInstance())
            {
                using (var runner = engine.GetRunner(testPackage))
                {
                    var xml = runner.Explore(TestFilter.Empty);
                    var json = ToJson(xml);
                }
            }
            return new TestSuite();
        }

        private static string ToJson(XmlNode exploredNodes)
        {
            var myData = exploredNodes.OuterXml;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(myData);
            return JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
        }

        private List<string> GetIds(TestSuite suite)
        {
            List<string> GetIdsFromCases(List<TestCase> testCases)
            {
                return testCases.Select(x => x.Id).ToList();
            }

            List<string> GetIdsFromSuites(List<TestSuite> testSuites)
            {
                var results = new List<string>();
                var r = testSuites.Select(GetIds).ToList();
                r.ForEach(x => results.AddRange(x));
                return results;
            }

            var result = new List<string>() {suite.Id};
            result.AddRange(GetIdsFromSuites(suite.TestSuites));
            result.AddRange(GetIdsFromCases(suite.TestCases));
            return result;
        }

        public (List<TestPackage> TestPackages, List<TestSuite> TestSuites, Dictionary<string, TestPackage>
            IdToTestPackageMap)
            GetInitializationParameters(params TestAssemblyLocationHolder[] testAssembliesLocationHolder)
        {
            List<TestPackage> testPackages =
                testAssembliesLocationHolder.Select(x => new TestPackage(x.Location)).ToList();

            Dictionary<TestPackage, TestSuite> packageToSuiteMap =
                testPackages.ToDictionary(x => x, GetTestSuite);

            List<TestSuite> testSuites = packageToSuiteMap.Values.ToList();

            Dictionary<string, TestPackage> map = GetMap(packageToSuiteMap);
            return (testPackages, testSuites, map);

        }

        private Dictionary<string, TestPackage> GetMap(Dictionary<TestPackage, TestSuite> packageAndSuites)
        {
            Dictionary<string, TestPackage> result = new Dictionary<string, TestPackage>();
            foreach (var packageAndSuite in packageAndSuites)
            {
                var ids = GetIds(packageAndSuite.Value);
                foreach (var id in ids)
                {
                    if(result.ContainsKey(id)) throw new Exception($"The testable id {id} is present more that one");
                    result.Add(id, packageAndSuite.Key);
                }
            }

            return result;
        }
    }
}