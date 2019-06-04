using Newtonsoft.Json;

namespace Eagle
{
    public class NUnitDiscoveryRoot
    {
        [JsonProperty("test-run")]
        public NUnitTestRun NUnitTestRun { get; set; }
    }
}