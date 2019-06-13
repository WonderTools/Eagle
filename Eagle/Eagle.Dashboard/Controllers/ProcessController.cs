using Microsoft.AspNetCore.Mvc;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/process")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        [HttpPost]
        public ActionResult Process()
        {
            return Ok();
        }
    }
}