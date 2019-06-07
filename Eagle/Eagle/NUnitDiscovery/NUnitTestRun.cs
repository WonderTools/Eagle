using Newtonsoft.Json;

namespace Eagle.NUnitDiscovery
{
    public class NUnitTestRun
    {
        [JsonProperty("test-suite")]
        public NUnitTestSuite TestSuite { get; set; }
    }
}