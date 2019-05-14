using NUnit.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Eagle
{
    public class EagleEngine
    {
        public List<NameAndId> GetFeatureNames()
        {
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var assemblies = GetAssemblies(currentDirectory);
            var validAssemblies = assemblies.Where(a => a.FullName.StartsWith("Feature.")).ToList();
            var featureAssemblies = validAssemblies.Where(IsAssemblyFeatureAssembly).ToList();
            return featureAssemblies.SelectMany(GetFeatureNames).ToList();
        }

        private  List<Assembly> GetAssemblies(string currentDirectory)
        {
            List<Assembly> assemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(currentDirectory);

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
                assemblies.Add(Assembly.LoadFile(dll));
            return assemblies;
        }

        private bool IsAssemblyFeatureAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(x => x.IsClass)
                .Where(x => x.GetCustomAttribute<FeaturePackageAttribute>() != null);
            if (types.Count() != 0) return true;
            return false;
        }


        private static string ToJson(XmlNode exploredNodes)
        {
            var myData = exploredNodes.OuterXml;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(myData);
            return JsonConvert.SerializeXmlNode(doc, Formatting.Indented);
        }

        private List<NameAndId> GetFeatureNames(Assembly assembly)
        {
            var testPackage = GetTestPackage(assembly);
            using (var engine = TestEngineActivator.CreateInstance())
            {
                using (var runner = engine.GetRunner(testPackage))
                {
                    var xml = runner.Explore(TestFilter.Empty);
                    var json = ToJson(xml);

                }
            }

            return new List<NameAndId>();
        }

        private TestPackage GetTestPackage(Assembly assembly)
        {
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.GetDirectoryName(currentDirectory);

            var assemblyName = path +"\\"+ assembly.GetName().Name + ".dll";
            TestPackage package = new TestPackage(assemblyName);
            
            return package;
        }
    }
}