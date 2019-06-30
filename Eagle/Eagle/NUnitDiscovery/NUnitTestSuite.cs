using System.Collections.Generic;
using Newtonsoft.Json;

namespace WonderTools.Eagle.Core.NUnitDiscovery
{
    public class NUnitTestSuite
    {
        [JsonProperty("@name")]
        public string Name { get; set; }

        [JsonProperty("@fullname")]
        public string Fullname { get; set; }

        [JsonConverter(typeof(ListOrObjectConverter<NUnitTestSuite>))]
        [JsonProperty("test-suite", NullValueHandling = NullValueHandling.Ignore)]
        public List<NUnitTestSuite> TestSuites { get; set; } = new List<NUnitTestSuite>();

        [JsonConverter(typeof(ListOrObjectConverter<NUnitTestCase>))]
        [JsonProperty("test-case", NullValueHandling = NullValueHandling.Ignore)]
        public List<NUnitTestCase> TestCases { get; set; } = new List<NUnitTestCase>();
    }
}