using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Web.Controllers
{
    [Route("api/test-tree/")]
    [ApiController]
    public class TestTreeController : ControllerBase
    {
        private readonly EagleEngine _eagleEngine;

        public TestTreeController(EagleEngine eagleEngine)
        {
            _eagleEngine = eagleEngine;
        }

        [HttpGet]
        public ActionResult<List<TestSuite>> GetFeatureNames()
        {
            return _eagleEngine.GetDiscoveredTestSuites();
        }
    }
}   