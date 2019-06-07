using System;
using Newtonsoft.Json;

namespace Eagle.NUnitResult
{
    public class NUnitResultTestCase
    {
        [JsonProperty("@fullname")]
        public string Fullname { get; set; }

        [JsonProperty("@result")]
        public string Result { get; set; }

        [JsonProperty("@start-time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("@end-time")]
        public DateTime EndTime { get; set; }

        [JsonProperty("@duration")]
        public double Duration { get; set; }

    }


 
}
