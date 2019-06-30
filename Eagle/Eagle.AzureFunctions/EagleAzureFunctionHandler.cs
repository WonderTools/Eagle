using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WonderTools.Eagle.Core;
using WonderTools.Eagle.Http.Contract;

namespace WonderTools.Eagle.Http
{
    public static class RequestHandler
    {
        public static async Task<IActionResult> HandleRequest(HttpRequest request, params TestableAssembly[] assemblies)
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
            return new OkObjectResult(JsonConvert.SerializeObject(result));
        }
    }
}
