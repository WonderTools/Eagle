using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<List<TestSuiteModel>> GetTestTree()
        {
            return _service.GetTestTree();
        }
    }
}   