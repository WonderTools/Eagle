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
        private TestRunner _testRunner;
        private List<TestSuite> _testSuites;
        private Dictionary<string,(TestPackage TestPackage, string FullName)> _idToSchedulingParametersMap;
        
        public EagleEngine(params TestableAssembly[] testableAssemblies)
        {
            Initialize(testableAssemblies);
        }

        private void Initialize(params TestableAssembly[] testableAssemblies)
        {
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
            return _testSuites;
        }
    }
}   