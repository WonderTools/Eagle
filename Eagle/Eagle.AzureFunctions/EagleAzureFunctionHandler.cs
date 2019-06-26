using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WonderTools.Eagle.Communication.Contract;
using Newtonsoft.Json;

namespace Eagle.AzureFunctions
{
    public class EagleAzureFunctionHandler
    {
        public async Task<TestReport> HandleRequest(HttpRequestMessage request, params TestableAssembly[] assemblies)
        {
            var serializedTrigger = await request.Content.ReadAsStringAsync();
            var trigger = JsonConvert.DeserializeObject<TestTrigger>(serializedTrigger);
            var engine = new EagleEngine(assemblies);
            var discoveredTestSuites = engine.GetDiscoveredTestSuites();
            var httpRequestResultHandler = new HttpRequestResultHandler(discoveredTestSuites, trigger.NodeName, trigger.RequestId, trigger.CallBackUrl);
            var results = await engine.ExecuteTest(httpRequestResultHandler, trigger.Id);
            return new TestReport()
            {
                NodeName = trigger.NodeName,
                RequestId = trigger.RequestId,
                TestResults = results,
                TestSuites = discoveredTestSuites
            };
        }
    }
}
