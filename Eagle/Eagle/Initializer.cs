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
                    var json = xml.ToJson();
                    var nUnitJsonParser = new NUnitJsonParser();
                    return nUnitJsonParser.GetTestSuiteFromDiscoveryJson(json);
                }
            }
        }

        private List<(string FullName, string Id)> GetFullNameAndIds(TestSuite suite)
        {
            List<(string FullName, string Id)> GetFullNameAndIdsFromCases(List<TestCase> testCases)
            {
                return testCases.Select(x => (x.FullName, x.Id)).ToList();
            }

            List<(string FullName, string Id)> GetFullNameAndIdsFromSuites(List<TestSuite> testSuites)
            {
                var results = new List<(string FullName, string Id)>();
                var r = testSuites.Select(GetFullNameAndIds).ToList();
                r.ForEach(x => results.AddRange(x));
                return results;
            }

            var result = new List<(string FullName, string Id)>() {(suite.FullName, suite.Id)};
            result.AddRange(GetFullNameAndIdsFromSuites(suite.TestSuites));
            result.AddRange(GetFullNameAndIdsFromCases(suite.TestCases));
            return result;
        }

        public Dictionary<TestPackage, TestSuite> GetTestPackageToTestSuiteMap
            (params TestAssemblyLocationHolder[] testAssembliesLocationHolder)
        {
            List<TestPackage> testPackages =
                testAssembliesLocationHolder.Select(x => new TestPackage(x.Location)).ToList();
            return testPackages.ToDictionary(x => x, GetTestSuite);
        }


        public Dictionary<string, (TestPackage TestPackage, string FullName)>
            GetIdToSchedulingParametersMap(Dictionary<TestPackage, TestSuite> packageToSuiteMap)
        {
            var result = new Dictionary<string, (TestPackage TestPackage, string Name)>();
            foreach (var packageAndSuite in packageToSuiteMap)
            {
                var fullNameAndIds = GetFullNameAndIds(packageAndSuite.Value);
                foreach (var fullNameAndId in fullNameAndIds)
                {
                    if (result.ContainsKey(fullNameAndId.Id)) throw new Exception($"The testable id {fullNameAndId.Id} is present more that one");
                    result.Add(fullNameAndId.Id, (packageAndSuite.Key, fullNameAndId.FullName));
                }
            }
            return result;
        }
    }
}