using System.Collections.Generic;
using System.Linq;
using Eagle.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Web.Controllers
{
    [Route("api/features")]
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
    }
}