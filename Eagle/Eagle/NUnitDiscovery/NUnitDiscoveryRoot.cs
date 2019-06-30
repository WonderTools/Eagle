using Newtonsoft.Json;

namespace WonderTools.Eagle.Core.NUnitDiscovery
{
    public class NUnitDiscoveryRoot
    {
        [JsonProperty("test-run")]
        public NUnitTestRun TestRun { get; set; }
    }
}