using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace PlayGround
{
    public class DeserializationTests
    {
        private string ReadEmbeddedResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = fileName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }


        [Test]
        public void Test()
        {
            var text = ReadEmbeddedResource("PlayGround.input.json");

            RootObject account = JsonConvert.DeserializeObject<RootObject>(text);


            var newText = JsonConvert.SerializeObject(account);
            Console.WriteLine(text);
        }
    }
}