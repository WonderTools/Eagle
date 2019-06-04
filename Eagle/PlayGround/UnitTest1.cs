using System;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using PlayGround;

namespace PlayGround
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]

        public void TestJson()
        {
            string json;
            using (StreamReader r = new StreamReader("file.json"))
            {
                json = r.ReadToEnd();
            }
            var deserializeJsonObject = JsonConvert.DeserializeObject<RootObject>(json);

            var serializeData = JsonConvert.SerializeObject(deserializeJsonObject);

            Console.WriteLine(serializeData);

        }
    }
}