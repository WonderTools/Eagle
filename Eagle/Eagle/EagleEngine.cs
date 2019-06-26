using NUnit.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Eagle.Contract;
using Newtonsoft.Json;


namespace Eagle
{
    public class EagleEngine
    {
        private List<TestPackage> _testPackages;
        private List<PackageAndSuite> _packageAndTestSuites;
        private Dictionary<string, ExecutableTest> _executableTests;

        private TestRunner _testRunner;
        private List<TestSuite> _testSuites;
        private Dictionary<string,(TestPackage TestPackage, string FullName)> _idToSchedulingParametersMap;
        
        public EagleEngine(params TestableAssembly[] testableAssemblies)
        {
            Initialize(testableAssemblies);
            _packageAndTestSuites = _testPackages.Select(p => new PackageAndSuite(){ TestPackage = p, TestSuite = GetTestSuite(p)}).ToList();
            _executableTests = GetExecutableTests(_packageAndTestSuites);

        }

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

        private Dictionary<string, ExecutableTest>
            GetExecutableTests(List<PackageAndSuite> packageAndSuites)
        {
            var result = new Dictionary<string, ExecutableTest>();
            foreach (var packageAndSuite in packageAndSuites)
            {
                var fullNameAndIds = GetFullNameAndIds(packageAndSuite.TestSuite);
                foreach (var fullNameAndId in fullNameAndIds)
                {
                    if (result.ContainsKey(fullNameAndId.Id)) throw new Exception($"The testable id {fullNameAndId.Id} is present more that one");
                    result.Add(fullNameAndId.Id, new ExecutableTest()
                    {
                        FullName = fullNameAndId.FullName,
                        Id = fullNameAndId.Id,
                        TestPackage = packageAndSuite.TestPackage
                    });
                }
            }
            return result;
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

            var result = new List<(string FullName, string Id)>() { (suite.FullName, suite.Id) };
            result.AddRange(GetFullNameAndIdsFromSuites(suite.TestSuites));
            result.AddRange(GetFullNameAndIdsFromCases(suite.TestCases));
            return result;
        }

        private void Initialize(params TestableAssembly[] testableAssemblies)
        {
            _testPackages =
                testableAssemblies.Select(x => new TestPackage(x.Location)).ToList();

            Initializer initializer = new Initializer();
            var packageToSuiteMap = initializer.GetTestPackageToTestSuiteMap(testableAssemblies);
            _testSuites = packageToSuiteMap.Values.ToList();
            _idToSchedulingParametersMap = initializer.GetIdToSchedulingParametersMap(packageToSuiteMap);
            _testRunner = new TestRunner(_idToSchedulingParametersMap);
        }

        public async Task<List<TestResult>> ExecuteTest(string id, IResultHandler resultHandler)
        {
            var result = await _testRunner.RunTestCaseNew(id);
            await resultHandler.OnTestCompletion(result);
            return result;
        }

        public List<TestSuite> GetDiscoveredTestSuites()
        {
            return _packageAndTestSuites.Select(x => x.TestSuite).ToList();
        }

        class ExecutableTest
        {
            public string FullName { get; set; }
            public string Id { get; set; }
            public TestPackage TestPackage { get; set; }
        }

        class PackageAndSuite
        {
            public TestPackage TestPackage { get; set; }
            public TestSuite TestSuite { get; set; }
        }
    }

    
}   