using AutoMapper;
using Newtonsoft.Json;

namespace Eagle.TestRun
{
    public class TestRunJsonParser
    {
        public RunTestSuite GetTestSuiteFromDiscoveryJson(string json)
        {
            var root = JsonConvert.DeserializeObject<TestRoot>(json);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestRunTestSuite, RunTestSuite>();
                cfg.CreateMap<TestRunTestCase,RunTestCase>();
            });

            return config.CreateMapper().Map<RunTestSuite>(root.TestRun.TestSuite);
        }
    }
}