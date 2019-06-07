using System;
using Newtonsoft.Json;

namespace Eagle.TestRun
{
    public class ResultTestCase
    {
        [JsonProperty("@fullname")]
        public string Fullname { get; set; }

        [JsonProperty("@result")]
        public string Result { get; set; }

        [JsonProperty("@start-time")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("@end-time")]
        public DateTimeOffset EndTime { get; set; }

        [JsonProperty("@duration")]
        public string Duration { get; set; }

    }


 
}
