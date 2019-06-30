using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WonderTools.Eagle.Contract;
using WonderTools.Eagle.Core;
using WonderTools.Eagle.Http.Contract;

namespace WonderTools.Eagle.Http.NUnit
{
    public class HttpRequestResultHandler : IResultHandler
    {
        private readonly List<TestSuite> _testSuites;
        private readonly string _nodeName;
        private readonly string _requestId;

        private readonly string _callbackUri;
        public HttpRequestResultHandler(List<TestSuite> testSuites, string nodeName, string requestId, string callbackUri)
        {
            _testSuites = testSuites;
            _nodeName = nodeName;
            _requestId = requestId;
            _callbackUri = callbackUri;
        }

        //TODO The exception should not be always swallowed. This should be swallowed only in development mode set by a configuration in appsettings.json 
        public async Task OnTestCompletion(List<TestResult> result)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var myResult = new TestReport();
                    myResult.RequestId = _requestId;
                    myResult.NodeName = _nodeName;
                    myResult.TestSuites = _testSuites;
                    myResult.TestResults = result;
                    var serializedResult = JsonConvert.SerializeObject(myResult);
                    var content = new StringContent(serializedResult);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await httpClient.PostAsync(_callbackUri, content);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}