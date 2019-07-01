using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WonderTools.Eagle.NUnit
{
    public class ListOrObjectConverter<T> : JsonConverter
    {
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
            var result = (T)serializer.Deserialize(reader, typeof(T));
            return new List<T> { result };
        }

        public override bool CanWrite { get; } = false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}