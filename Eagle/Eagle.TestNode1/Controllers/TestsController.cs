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
            var eagleEngine = new EagleEngine(typeof(TestClass));

            var testSuites = eagleEngine.GetDiscoveredTestSuites();
            IResultHandler handler = new HttpRequestResultHandler(testSuites, value.NodeName, value.RequestId, value.CallBackUrl);
            var result = await eagleEngine.ExecuteTest(value.Id,handler);

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
