using Newtonsoft.Json;

namespace PlayGround
{
    

    public class TestRoot
    {
        [JsonProperty("test-run")]
        public TestRun TestRun { get; set; }
    }
}