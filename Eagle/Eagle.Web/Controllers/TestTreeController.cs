using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eagle.Web.Models;
using Eagle.Web.Service;
using Microsoft.AspNetCore.Mvc;

namespace Eagle.Web.Controllers
{
    [Route("api/test-tree/")]
    [ApiController]
    public class TestTreeController : ControllerBase
    {
        private readonly TestTreeService _service;

        public TestTreeController(TestTreeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TestSuiteModel>>> GetTestTree()
        {
            return await _service.GetTestTree();
        }
    }
}   