using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.WebTemp;
using Feature.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.TestNode1.Controllers
{

    [Route("api/tests")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        
        [HttpPost("execute")]
        public async Task<MyResult> Execute([FromBody] TestTrigger value)
        {
            IResultHandler handler = new HttpRequestResultHandler();
            var eagleEngine = new EagleEngine(typeof(TestClass));
            var testSuites = eagleEngine.GetDiscoveredTestSuites();
            var result = await eagleEngine.ExecuteTest(value.Id, value.NodeName, value.RequestId, value.CallBackUrl, handler);

            return new MyResult()
            {
                TestResults = result,
                TestSuites = testSuites,
                NodeName = value.NodeName,
                RequestId = value.RequestId,
            };
        }
    }
}
