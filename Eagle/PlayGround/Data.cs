using System.Collections.Generic;
using System;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace PlayGround
{
    
    public class RootObject
    {
        [JsonProperty("test-run")]
        public TestRun TestRun { get; set; }
    }

    public class TestRun
    {
        [JsonProperty("@fullname")]
        public string Fullname { get; set; }

        [JsonProperty("test-suite")]
        public TestRunTestSuite TestSuite { get; set; }
    }

    public class TestRunTestSuite
    {
        [JsonProperty("@fullname")]
        public string Fullname { get; set; }

        [JsonProperty("@runstate")]
        public string Runstate { get; set; }

        [JsonConverter(typeof(MyConverter<TestRunTestSuite>))]
        [JsonProperty("test-suite")]
        public List<TestRunTestSuite> TestSuites { get; set; }

        [JsonConverter(typeof(MyConverter<TestCaseElement>))]
        [JsonProperty("test-case")]
        public List<TestCaseElement> TestCases { get; set; }
    }

    public class MyConverter<T> : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize(reader, objectType);
            }
            return new List<T> { (T)serializer.Deserialize(reader, typeof(T)) };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


    public class TestCaseElement
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@fullname")]
        public string Fullname { get; set; }
    }
}