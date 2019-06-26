using Newtonsoft.Json;

namespace WonderTools.Eagle.NUnitDiscovery
{
    public class NUnitDiscoveryRoot
    {
        [JsonProperty("test-run")]
        public NUnitTestRun TestRun { get; set; }
    }
}