using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.TestNode2.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestsController : ControllerBase
    {

        [HttpPost("execute")]
        public async Task<MyResult> Execute([FromBody] ExecuteParameters value)
        {
            IResultHandler handler = new HttpRequestResultHandler();
            var eagleEngine = new EagleEngine(new MyLogger(), handler);
            eagleEngine.Initialize(new EagleEventListener(), typeof(AzureResponseCodeTests));
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

    public class MyLogger : IMyLogger
    {
        public void Log(string log)
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