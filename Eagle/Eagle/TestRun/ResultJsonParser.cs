using AutoMapper;
using Newtonsoft.Json;

namespace Eagle.TestRun
{
    public class ResultJsonParser
    {
        public RunTestSuite GetTestSuiteFromDiscoveryJson(string json)
        {
            var root = JsonConvert.DeserializeObject<ResultRoot>(json);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ResultTestSuite, RunTestSuite>();
                cfg.CreateMap<ResultTestCase,RunTestCase>();
            });

            return config.CreateMapper().Map<RunTestSuite>(root.TestRun.TestSuite);
        }
    }
}