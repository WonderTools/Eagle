using Newtonsoft.Json;

namespace Eagle.NUnitResult
{
    public class NUnitResultRun
    {
        [JsonProperty("test-suite")]
        public NUnitResultTestSuite TestSuite { get; set; }
    }
}