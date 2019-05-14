using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost("process")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Process()
        {
            Console.WriteLine("Triggered ***" );
            return Ok();
        }


        [HttpGet]
        public ActionResult<List<FeatureIdAndNames>> GetFeatureNames()
        {
            return _eagleEngine.GetFeatureNames()
                .Select(x => new FeatureIdAndNames() { Id = x.Id, Name = x.Name})
                .ToList();
            
        }
    }
}