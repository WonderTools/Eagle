using System.Threading.Tasks;
using Eagle.Dashboard.Models;
using Eagle.Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/nodes")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public NodesController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateNode(NodeCreationParameters creationParameters)
        {
            await _dashboardService.CreateNode(creationParameters);
            return Ok();
        }
    }
}