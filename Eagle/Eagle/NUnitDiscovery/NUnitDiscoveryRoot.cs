using Newtonsoft.Json;

namespace WonderTools.Eagle.NUnit.NUnitDiscovery
{
    public class NUnitDiscoveryRoot
    {
        [JsonProperty("test-run")]
        public NUnitTestRun TestRun { get; set; }
    }
}