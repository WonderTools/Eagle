using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlayGround
{
    public class TestRunTestSuite
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

        [JsonConverter(typeof(ListOrObjectConverter<TestRunTestSuite>))]
        [JsonProperty("test-suite", NullValueHandling = NullValueHandling.Ignore)]
        public List<TestRunTestSuite> TestSuites { get; set; } = new List<TestRunTestSuite>();

        [JsonConverter(typeof(ListOrObjectConverter<TestRunTestCase>))]
        [JsonProperty("test-case", NullValueHandling = NullValueHandling.Ignore)]
        public List<TestRunTestCase> TestCases { get; set; } = new List<TestRunTestCase>();


    }
}