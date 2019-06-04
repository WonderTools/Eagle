using Newtonsoft.Json;

namespace Eagle
{
    public class NUnitTestRun
    {
        [JsonProperty("test-suite")]
        public NUnitTestSuite NUnitTestSuite { get; set; }
    }
}