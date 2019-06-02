using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Web.Controllers
{
    [Route("api/test-tree/")]
    [ApiController]
    public class Tests : ControllerBase
    {
        private readonly EagleEngine _eagleEngine;

        public Tests(EagleEngine eagleEngine)
        {
            _eagleEngine = eagleEngine;
        }

        [HttpGet]
        public ActionResult<List<TestSuite>> GetFeatureNames()
        {
            return _eagleEngine.GetDiscoveredTestSuites();
        }


        //[HttpPost("{id}/schedule")]
        //public ActionResult<string> ScheduleFeature(string id)
        //{
        //    _eagleEngine.ScheduleFeature(id);
        //    return Ok();
        //}


    }
}   