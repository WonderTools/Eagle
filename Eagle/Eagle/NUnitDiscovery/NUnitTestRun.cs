using Newtonsoft.Json;

namespace WonderTools.Eagle.NUnit.NUnitDiscovery
{
    public class NUnitTestRun
    {
        [JsonProperty("test-suite")]
        public NUnitTestSuite TestSuite { get; set; }
    }
}