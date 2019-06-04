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
                //TBD: Currently all spaces are replace. This must be changed as per url encoding
                //TBD: Temporarily id is prefixed
                cfg.CreateMap<NUnitTestSuite, TestSuite>();
                    //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => "id-"+ src.Name.Replace(" ", "")));

                cfg.CreateMap<NUnitTestCase, TestCase>();
                    //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => "id-" + src.Name.Replace(" ", "")));
            });

            var testSuite =  config.CreateMapper().Map <TestSuite>(root.TestRun.TestSuite);
            return testSuite;
        }
    }
}