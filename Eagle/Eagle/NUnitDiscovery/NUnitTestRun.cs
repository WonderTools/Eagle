using Newtonsoft.Json;

namespace WonderTools.Eagle.Core.NUnitDiscovery
{
    public class NUnitTestRun
    {
        [JsonProperty("test-suite")]
        public NUnitTestSuite TestSuite { get; set; }
    }
}