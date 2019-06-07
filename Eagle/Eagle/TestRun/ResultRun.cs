using Newtonsoft.Json;

namespace Eagle.TestRun
{
    public class ResultRun
    {
        [JsonProperty("test-suite")]
        public ResultTestSuite TestSuite { get; set; }
    }
}