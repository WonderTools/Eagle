using Newtonsoft.Json;

namespace Eagle.NUnitResult
{
    

    public class NUnitResultRoot
    {
        [JsonProperty("test-run")]
        public NUnitResultRun TestRun { get; set; }
    }
}