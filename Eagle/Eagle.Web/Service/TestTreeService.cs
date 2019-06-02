using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eagle.Web.Models;

namespace Eagle.Web.Service
{
    public class TestTreeService
    {
        private readonly EagleEngine _eagleEngine;

        public TestTreeService(EagleEngine eagleEngine)
        {
            _eagleEngine = eagleEngine;
        }

        public List<TestSuiteModel> GetTestTree()
        {
            var testSuites = _eagleEngine.GetDiscoveredTestSuites();
            var testSuiteModels = Convert(testSuites);


            return testSuiteModels;
        }

        private List<TestSuiteModel> Convert(List<TestSuite> suites)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestSuite, TestSuiteModel>();
                cfg.CreateMap<TestCase, TestCaseModel>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<List<TestSuiteModel>>(suites);
        }
    }
}
