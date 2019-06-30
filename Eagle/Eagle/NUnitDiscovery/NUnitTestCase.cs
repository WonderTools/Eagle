using Newtonsoft.Json;

namespace WonderTools.Eagle.Core.NUnitDiscovery
{
    public class NUnitTestCase
    {
        [JsonProperty("@name")]
        public string Name { get; set; }

        [JsonProperty("@fullname")]
        public string Fullname { get; set; }
    }
}