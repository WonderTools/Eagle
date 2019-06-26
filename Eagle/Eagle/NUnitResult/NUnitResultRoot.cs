using Newtonsoft.Json;

namespace WonderTools.Eagle.NUnitResult
{
    

    public class NUnitResultRoot
    {
        [JsonProperty("test-run")]
        public NUnitResultRun TestRun { get; set; }
    }
}