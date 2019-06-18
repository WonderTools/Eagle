using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feature.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.TestNode1.Controllers
{

    [Route("api/tests")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        
        [HttpPost("execute")]
        public async Task<MyResult> Execute([FromBody] ExecuteParameters value)
        {
            IResultHandler handler = new HttpRequestResultHandler();
            var eagleEngine = new EagleEngine(handler);
            eagleEngine.Initialize(new EagleEventListener(), typeof(TestClass));
            var result = await eagleEngine.ExecuteTest(value.Id, value.NodeName, value.RequestId, value.CallBackUrl);
            return result;
            
        }
    }



    public class EagleEventListener : IEagleEventListener
    {
        public async Task TestCompleted(string id, string result, DateTime startingTime, DateTime finishingTime, int duration)
        {
            
        }
    }

    public class ExecuteParameters
    {
        public string NodeName { get; set; }
        public string Id { get; set; }
        public string CallBackUrl { get; set; }
        public string RequestId { get; set; }
    }
}
