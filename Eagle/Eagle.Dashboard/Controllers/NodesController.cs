using Eagle.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/nodes")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreateNode(NodeCreationParameters creationParameters)
        {
            return Ok();
        }
    }
}