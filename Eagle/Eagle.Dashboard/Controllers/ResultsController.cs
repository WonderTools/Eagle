using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/results")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        [HttpPost]
        public ActionResult AddResults(MyResult result)
        {
            var serializedResult = JsonConvert.SerializeObject(result);
            return Ok();
        }
    }
}