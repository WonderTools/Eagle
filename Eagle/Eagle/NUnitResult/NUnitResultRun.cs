using Newtonsoft.Json;

namespace WonderTools.Eagle.Core.NUnitResult
{
    public class NUnitResultRun
    {
        [JsonProperty("test-suite")]
        public NUnitResultTestSuite TestSuite { get; set; }
    }
}