using System;
using System.IO;
using System.Threading.Tasks;
using Feature.Infrastructure;
using WonderTools.Eagle.AzureFunctions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WonderTools.Eagle;
using WonderTools.Eagle.Communication.Contract;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return await HandleRequest(req, typeof(TestMyClass));


            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

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
