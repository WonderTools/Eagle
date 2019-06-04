using System.Collections.Generic;
using Newtonsoft.Json;

namespace Eagle
{
    public class NUnitTestSuite
    {

        [JsonProperty("@name")]
        public string Name { get; set; }

        [JsonProperty("@fullname")]
        public string Fullname { get; set; }

        [JsonConverter(typeof(ListOrObjectConverter<NUnitTestSuite>))]
        [JsonProperty("test-suite", NullValueHandling = NullValueHandling.Ignore)]
        public List<NUnitTestSuite> TestSuites { get; set; }

        [JsonConverter(typeof(ListOrObjectConverter<NUnitTestCase>))]
        [JsonProperty("test-case", NullValueHandling = NullValueHandling.Ignore)]
        public List<NUnitTestCase> TestCases { get; set; }
    }
}