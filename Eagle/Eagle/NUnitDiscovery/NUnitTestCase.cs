using Newtonsoft.Json;

namespace Eagle.NUnitDiscovery
{
    public class NUnitTestCase
    {
        [JsonProperty("@name")]
        public string Name { get; set; }

        [JsonProperty("@fullname")]
        public string Fullname { get; set; }
    }
}