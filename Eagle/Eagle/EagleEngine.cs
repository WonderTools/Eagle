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

        public async Task<MyResult> ExecuteTest(string id, string nodeName, string requestId, string uri, IResultHandler resultHandler)
        {

            //TODO When Id is empty all test should be executed
            var result = await _testRunner.RunTestCaseNew(id);
            var discoveredTestSuites = GetDiscoveredTestSuites();
            var executeTest = new MyResult()
            {
                TestResults = result,
                TestSuites = discoveredTestSuites,
                NodeName =  nodeName,
                RequestId= requestId,
            };
            await resultHandler.OnTestCompletion(uri, executeTest);
            return executeTest;
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


    
    public class HttpRequestResultHandler : IResultHandler
    {
        //TODO The exception should not be always swallowed. This should be swallowed only in development mode set by a configuration in appsettings.json 
        public async Task OnTestCompletion(string listenerUri, MyResult result)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var serializedResult = JsonConvert.SerializeObject(result);
                    var content = new StringContent(serializedResult);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response =  await httpClient.PostAsync(listenerUri, content);
                }
            }
            catch (Exception e)
            {
                
            }
            
        }
    }
}   