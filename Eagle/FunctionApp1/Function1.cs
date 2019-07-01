using System;
using System.IO;
using System.Threading.Tasks;
using Feature.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WonderTools.Eagle;
using WonderTools.Eagle.Http;
using WonderTools.Eagle.Http.NUnit;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return await req.HandleRequest(typeof(TestMyClass));
        }
    }
}
