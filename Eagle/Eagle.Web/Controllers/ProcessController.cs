using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Web.Controllers
{
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