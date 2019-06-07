using Newtonsoft.Json;

namespace Eagle.NUnitDiscovery
{
    public class NUnitDiscoveryRoot
    {
        [JsonProperty("test-run")]
        public NUnitTestRun TestRun { get; set; }
    }
}