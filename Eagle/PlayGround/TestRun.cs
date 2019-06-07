using Newtonsoft.Json;

namespace PlayGround
{
    public class TestRun
    {
        [JsonProperty("test-suite")]
        public TestRunTestSuite TestSuite { get; set; }
    }
}