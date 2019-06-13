using Microsoft.AspNetCore.Mvc;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/results")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        [HttpPost]
        public ActionResult AddResults(MyResult result)
        {
            return Ok();
        }
    }
}