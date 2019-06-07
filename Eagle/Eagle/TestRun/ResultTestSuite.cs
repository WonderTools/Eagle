using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Eagle.TestRun
{
    public class ResultTestSuite
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

        [JsonConverter(typeof(ListOrObjectConverter<ResultTestSuite>))]
        [JsonProperty("test-suite", NullValueHandling = NullValueHandling.Ignore)]
        public List<ResultTestSuite> TestSuites { get; set; } = new List<ResultTestSuite>();

        [JsonConverter(typeof(ListOrObjectConverter<ResultTestCase>))]
        [JsonProperty("test-case", NullValueHandling = NullValueHandling.Ignore)]
        public List<ResultTestCase> TestCases { get; set; } = new List<ResultTestCase>();


    }
}