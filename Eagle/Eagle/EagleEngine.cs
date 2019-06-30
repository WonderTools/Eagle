using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Engine;
using WonderTools.Eagle.Contract;

namespace WonderTools.Eagle.Core
{
    public class EagleEngine
    {
        private List<TestPackage> _testPackages;
        private List<PackageAndSuite> _packageAndTestSuites;
        private Dictionary<string, ExecutableTest> _executableTests;

        public EagleEngine(params TestableAssembly[] testableAssemblies)
        {
            _testPackages =
                testableAssemblies.Select(x => new TestPackage(x.Location)).ToList();
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

        private Dictionary<string, ExecutableTest> GetExecutableTests(List<PackageAndSuite> packageAndSuites)
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

        public Task<List<TestResult>> ExecuteTest(IResultHandler resultHandler, string id = null)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ExecuteAllTests(resultHandler);
            }
            else
            {
                return ExecuteTestInternal(resultHandler, id);
            }
        }

        private async Task<List<TestResult>> ExecuteTestInternal(IResultHandler resultHandler, string id)
        {
            var testRunner = new TestRunner();
            if(!_executableTests.ContainsKey(id)) throw new Exception("Test id not found");
            var executableTest = _executableTests[id];

            var result = await testRunner.RunTestCase(executableTest.TestPackage, executableTest.FullName);
            await resultHandler.OnTestCompletion(result);
            return result;
        }


        private async Task<List<TestResult>> ExecuteAllTests(IResultHandler resultHandler)
        {
            var result = new List<TestResult>();
            foreach (var testPackage in _testPackages)
            {
                var testRunner = new TestRunner();
                var r = await testRunner.RunTestCase(testPackage);
                result.AddRange(r);
            }
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