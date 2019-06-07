using AutoMapper;
using Newtonsoft.Json;

namespace Eagle.NUnitResult
{
    public class ResultJsonParser
    {
        public RunTestSuite GetTestSuiteFromDiscoveryJson(string json)
        {
            var root = JsonConvert.DeserializeObject<NUnitResultRoot>(json);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NUnitResultTestSuite, RunTestSuite>();
                cfg.CreateMap<NUnitResultTestCase,RunTestCase>();
            });

            return config.CreateMapper().Map<RunTestSuite>(root.TestRun.TestSuite);
        }
    }
}