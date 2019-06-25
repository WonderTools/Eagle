using System;
using System.Threading.Tasks;
using Eagle.WebTemp;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.TestNode2.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestsController : ControllerBase
    {

        [HttpPost("execute")]
        public async Task<MyResult> Execute([FromBody] TestTrigger value)
        {
            IResultHandler handler = new HttpRequestResultHandler();
            var eagleEngine = new EagleEngine(typeof(AzureResponseCodeTests));
            var result = await eagleEngine.ExecuteTest(value.Id, value.NodeName, value.RequestId, value.CallBackUrl, handler);
            return result;

        }
    }

    


}