using System.Collections.Generic;
using System.Threading.Tasks;
using Eagle.Dashboard.Models;
using Eagle.Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestsController: ControllerBase
    {
        private readonly DashboardService _service;

        public TestsController(DashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TestSuiteModel>>> GetTests()
        {
            var results = await  _service.GetResults();
            return Ok(results);
        }

        [HttpPost("schedule")]
        public async Task<ActionResult> ScheduleTests([FromBody]string id)
        {
            await _service.ScheduleTests(id);
            return Ok();
        }
    }
}