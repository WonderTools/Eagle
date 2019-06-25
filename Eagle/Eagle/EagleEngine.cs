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

        public async Task<List<TestResult>> ExecuteTest(string id, string nodeName, string requestId, string uri, IResultHandler resultHandler)
        {
            return await _testRunner.RunTestCaseNew(id);
        }

        public List<TestSuite> GetDiscoveredTestSuites()
        {
            return _testSuites;
        }


    }

    public class MyResult
    {
        public List<TestSuite> TestSuites { get; set; }

        public List<TestResult> TestResults { get; set; }

        public string NodeName { get; set; }
        public string RequestId { get; set; }
    }

    public interface IResultHandler
    {
        Task OnTestCompletion(string listenerUri, MyResult result);
    }
}   