﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feature.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WonderTools.Eagle;
using WonderTools.Eagle.Http;
using WonderTools.Eagle.Http.Contract;
using WonderTools.Eagle.Http.NUnit;
using WonderTools.Eagle.NUnit;

namespace Eagle.TestNode1.Controllers
{

    [Route("api/tests")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        
        [HttpPost("execute")]
        public async Task<TestReport> Execute([FromBody] TestTrigger value)
        {
            var eagleEngine = new EagleEngine(typeof(TestClass));

            var testSuites = eagleEngine.GetDiscoveredTestSuites();
            IResultHandler handler = new HttpRequestResultHandler(testSuites, value.NodeName, value.RequestId, value.CallBackUrl);
            var result = await eagleEngine.ExecuteTest(handler, value.Id);

            return new TestReport()
            {
                TestResults = result,
                TestSuites = testSuites,
                NodeName = value.NodeName,
                RequestId = value.RequestId,
            };
        }
    }
}
