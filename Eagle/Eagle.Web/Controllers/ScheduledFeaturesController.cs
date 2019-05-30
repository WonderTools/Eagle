using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Web.Controllers
{
    [Route("api/scheduled-features/")]
    [ApiController]
    public class ScheduledFeaturesController : ControllerBase
    {
        private readonly EagleEngine _eagleEngine;

        public ScheduledFeaturesController(EagleEngine eagleEngine)
        {
            _eagleEngine = eagleEngine;
        }

        [HttpGet]
        public ActionResult<List<ScheduledFeature>> GetScheduledFeatures()
        {
            var result = _eagleEngine.GetScheduledFeatures();
            return Ok(result);
        }
    }
}