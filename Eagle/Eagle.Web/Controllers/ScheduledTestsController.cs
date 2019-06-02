using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Web.Controllers
{
    [Route("api/scheduled-tests/")]
    [ApiController]
    public class ScheduledTestsController : ControllerBase
    {
        private readonly EagleEngine _eagleEngine;

        public ScheduledTestsController(EagleEngine eagleEngine)
        {
            _eagleEngine = eagleEngine;
        }

        [HttpGet]
        public ActionResult<List<ScheduledFeature>> GetScheduledFeatures()
        {
            var result = _eagleEngine.GetScheduledFeatures();
            return Ok(result);
        }

        [HttpPost]
        public ActionResult ScheduleTest([FromBody]string id)
        {
            var result = _eagleEngine.ScheduleTest(id);
            return Ok(result);
        }
    }
}