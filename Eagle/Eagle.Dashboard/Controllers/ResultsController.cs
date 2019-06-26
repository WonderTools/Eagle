using System.Threading.Tasks;
using WonderTools.Eagle.Communication.Contract;
using Eagle.Dashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/results")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly DashboardService _service;

        public ResultsController(DashboardService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> AddResults(TestReport result)
        {
            await _service.AddResults(result);
            return Ok();
        }
    }
}