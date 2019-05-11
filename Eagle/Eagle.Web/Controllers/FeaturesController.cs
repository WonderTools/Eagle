using System.Collections.Generic;
using Eagle.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Controllers
{
    [Route("api/features")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<FeatureIdAndNames>> GetFeatureNames()
        {
            return new List<FeatureIdAndNames>();
        }
    }
}