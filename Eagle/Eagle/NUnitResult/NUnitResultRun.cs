using Newtonsoft.Json;

namespace WonderTools.Eagle.NUnitResult
{
    public class NUnitResultRun
    {
        [JsonProperty("test-suite")]
        public NUnitResultTestSuite TestSuite { get; set; }
    }
}