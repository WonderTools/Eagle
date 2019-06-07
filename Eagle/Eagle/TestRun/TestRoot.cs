using Newtonsoft.Json;

namespace Eagle.TestRun
{
    

    public class TestRoot
    {
        [JsonProperty("test-run")]
        public TestRun TestRun { get; set; }
    }
}