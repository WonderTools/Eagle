using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

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

    [Route("api/nodes")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreateNode()
        {
            return Ok();
        }
    }
}
