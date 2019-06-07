using Newtonsoft.Json;

namespace Eagle.TestRun
{
    

    public class ResultRoot
    {
        [JsonProperty("test-run")]
        public ResultRun TestRun { get; set; }
    }
}