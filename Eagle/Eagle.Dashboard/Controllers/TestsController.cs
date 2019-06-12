using Microsoft.AspNetCore.Mvc;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestsController: ControllerBase
    {
        [HttpGet]
        public ActionResult GetTests()
        {
            return Ok();
        }

        [HttpPost("schedule")]
        public ActionResult ScheduleTests([FromBody]string id)
        {
            return Ok();
        }


    }
}