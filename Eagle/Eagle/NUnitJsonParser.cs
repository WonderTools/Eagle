using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;

namespace Eagle
{
    public class NUnitJsonParser
    {
        public TestSuite GetTestSuiteFromDiscoveryJson(string json)
        {
            var root = JsonConvert.DeserializeObject<NUnitDiscoveryRoot>(json);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NUnitTestSuite, TestSuite>();
                cfg.CreateMap<NUnitTestCase, TestCase>();
            });

            return config.CreateMapper().Map<TestSuite>(root.TestRun.TestSuite);
        }
    }
}