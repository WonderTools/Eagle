using AutoMapper;
using Eagle.NUnitDiscovery;
using Eagle.NUnitResult;
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

        public RunTestSuite GetTestSuiteFromResultJson(string json)
        {
            var root = JsonConvert.DeserializeObject<NUnitResultRoot>(json);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NUnitResultTestSuite, RunTestSuite>();
                cfg.CreateMap<NUnitResultTestCase, RunTestCase>();
            });

            return config.CreateMapper().Map<RunTestSuite>(root.TestRun.TestSuite);
        }
    }
}