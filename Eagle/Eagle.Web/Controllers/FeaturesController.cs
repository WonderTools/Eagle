using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Web.Controllers
{
    [Route("api/features/")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly EagleEngine _eagleEngine;

        public FeaturesController(EagleEngine eagleEngine)
        {
            _eagleEngine = eagleEngine;
        }

        [HttpGet]
        public ActionResult<List<FeatureIdAndNames>> GetFeatureNames()
        {
            return _eagleEngine.GetFeatureNames()
                .Select(x => new FeatureIdAndNames() { Id = x.Id, Name = x.Name})
                .ToList();
            
        }


        [HttpPost("{id}/schedule")]
        public ActionResult<string> ScheduleFeature(string id)
        {
            _eagleEngine.ScheduleFeature(id);
            return Ok();
        }

           
    }

    [Route("api/process/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProcessController : Controller
    {
        private readonly EagleEngine _eagleEngine;

        public ProcessController(EagleEngine eagleEngine)
        {
            _eagleEngine = eagleEngine;
        }


        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> Process()
        {
            await _eagleEngine.Process();
            return Ok();
        }
    }

}   