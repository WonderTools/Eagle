using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WonderTools.Eagle.Communication.Contract;

namespace WonderTools.Eagle.AzureFunctions
{
    public class EagleAzureFunctionHandler
    {
        public async Task<IActionResult> HandleRequest(HttpRequest request, params TestableAssembly[] assemblies)
        {
            var serializedTrigger = await new StreamReader(request.Body).ReadToEndAsync();
            var trigger = JsonConvert.DeserializeObject<TestTrigger>(serializedTrigger);
            var engine = new EagleEngine(assemblies);
            var discoveredTestSuites = engine.GetDiscoveredTestSuites();
            var httpRequestResultHandler = new HttpRequestResultHandler(discoveredTestSuites, trigger.NodeName, trigger.RequestId, trigger.CallBackUrl);
            var results = await engine.ExecuteTest(httpRequestResultHandler, trigger.Id);
            var result = new TestReport()
            {
                NodeName = trigger.NodeName,
                RequestId = trigger.RequestId,
                TestResults = results,
                TestSuites = discoveredTestSuites
            };
            return new OkObjectResult($"Hello");
        }
    }
}
