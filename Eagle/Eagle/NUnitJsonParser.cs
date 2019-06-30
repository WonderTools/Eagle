using AutoMapper;
using Newtonsoft.Json;
using WonderTools.Eagle.Contract;
using WonderTools.Eagle.Core.NUnitDiscovery;
using WonderTools.Eagle.Core.NUnitResult;

namespace WonderTools.Eagle.Core
{
    public class NUnitJsonParser
    {
        public TestSuite GetTestSuiteFromDiscoveryJson(string json)
        {
            var root = JsonConvert.DeserializeObject<NUnitDiscoveryRoot>(json);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NUnitTestSuite, TestSuite>()
                    .ForMember(o => o.Id, b => b.MapFrom(z => z.Fullname.GetIdFromFullName()));
                cfg.CreateMap<NUnitTestCase, TestCase>()
                    .ForMember(o => o.Id, b => b.MapFrom(z => z.Fullname.GetIdFromFullName()));
            });

            return config.CreateMapper().Map<TestSuite>(root.TestRun.TestSuite);
        }

        public ResultTestSuite GetTestSuiteFromResultJson(string json)
        {
            var root = JsonConvert.DeserializeObject<NUnitResultRoot>(json);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NUnitResultTestSuite, ResultTestSuite>();
                cfg.CreateMap<NUnitResultTestCase, ResultTestCase>();
            });

            return config.CreateMapper().Map<ResultTestSuite>(root.TestRun.TestSuite);
        }
    }
}