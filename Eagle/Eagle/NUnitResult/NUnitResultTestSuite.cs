using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WonderTools.Eagle.Core.NUnitResult
{
    public class NUnitResultTestSuite
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

        [JsonConverter(typeof(ListOrObjectConverter<NUnitResultTestSuite>))]
        [JsonProperty("test-suite", NullValueHandling = NullValueHandling.Ignore)]
        public List<NUnitResultTestSuite> TestSuites { get; set; } = new List<NUnitResultTestSuite>();

        [JsonConverter(typeof(ListOrObjectConverter<NUnitResultTestCase>))]
        [JsonProperty("test-case", NullValueHandling = NullValueHandling.Ignore)]
        public List<NUnitResultTestCase> TestCases { get; set; } = new List<NUnitResultTestCase>();


    }
}