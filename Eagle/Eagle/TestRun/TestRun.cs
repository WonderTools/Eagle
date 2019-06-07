using Newtonsoft.Json;

namespace Eagle.TestRun
{
    public class TestRun
    {
        [JsonProperty("test-suite")]
        public TestRunTestSuite TestSuite { get; set; }
    }
}