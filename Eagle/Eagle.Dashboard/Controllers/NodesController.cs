using System.Collections.Generic;
using System.Threading.Tasks;
using Eagle.Dashboard.Models;
using Eagle.Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/nodes/")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public NodesController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateNode(NodeUpsertParameters creationParameters)
        {
            if (ModelState.IsValid)
            {
                await _dashboardService.CreateNode(creationParameters);
            }
             return Ok();            

        }

        [HttpGet]
        public async Task<ActionResult<List<Node>>> GetNodes()
        {
            var results = await _dashboardService.GetNodes();
            return Ok(results);
        }

        [HttpPut("{nodeName}")]
        public async Task<ActionResult<Node>> UpdateNode(string nodeName, NodeUpsertParameters nodeUpsertParameters)
        {
            var node = await _dashboardService.UpdateNode(nodeName, nodeUpsertParameters);
            if (node != null)
            {
                return Ok(node);               
            }
            else
            {
                return NotFound();
            }
            
        }

        //delete
        [HttpDelete("{nodeName}")]
        public async Task<bool> DeleteNode(string nodeName)
        {
          return await _dashboardService.DeleteNode(nodeName);
        }
    }
}