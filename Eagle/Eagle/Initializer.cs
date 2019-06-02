using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using NUnit.Engine;
using Formatting = System.Xml.Formatting;

namespace Eagle
{
    public class Initializer
    {
        private TestSuite GetTestSuite(TestPackage testPackage)
        {
            using (var engine = TestEngineActivator.CreateInstance())
            {
                using (var runner = engine.GetRunner(testPackage))
                {
                    var xml = runner.Explore(TestFilter.Empty);
                    var json = xml.ToJson();
                    return GetHardCodedTestSuite(json);
                }
            }
        }

        public static TestSuite GetHardCodedTestSuite(string json)
        {
            //TBD: This has to be refactored
            return new TestSuite()
            {
                Name = "Feature.Infrastructure.dll",
                FullName = "Feature.Infrastructure.dll",
                TestSuites = new List<TestSuite>()
                {
                    new TestSuite()
                    {
                        Name = "Feature",
                        FullName = "Feature",
                        TestSuites = new List<TestSuite>()
                        {
                            new TestSuite()
                            {
                                Name = "Infrastructure",
                                FullName = "Feature.Infrastructure",
                                TestSuites = new List<TestSuite>()
                                {
                                    new TestSuite()
                                    {
                                        Name = "EagleFeature",
                                        FullName = "Feature.Infrastructure.EagleFeature",
                                        TestCases = new List<TestCase>()
                                        {
                                            new TestCase()
                                            {
                                                Name = "AddTwoNumbers",
                                                FullName = "Feature.Infrastructure.EagleFeature.AddTwoNumbers",
                                            },
                                            new TestCase()
                                            {
                                                Name = "SubtractTwoNumbers",
                                                FullName = "Feature.Infrastructure.EagleFeature.SubtractTwoNumbers",
                                            },
                                        }
                                    },
                                    new TestSuite()
                                    {
                                        Name = "TestClass",
                                        FullName = "Feature.Infrastructure.TestClass",
                                        TestCases = new List<TestCase>()
                                        {
                                            new TestCase()
                                            {
                                                Name = "TestMethod",
                                                FullName = "Feature.Infrastructure.TestClass.TestMethod",
                                            },
                                        }
                                    },
                                    new TestSuite()
                                    {
                                        Name = "TestMyClass",
                                        FullName = "Feature.Infrastructure.TestMyClass",
                                        TestCases = new List<TestCase>()
                                        {
                                            new TestCase()
                                            {
                                                Name = "MyTest(1)",
                                                FullName = "Feature.Infrastructure.TestMyClass.MyTest(1)",
                                            },
                                            new TestCase()
                                            {
                                                Name = "MyTest(2)",
                                                FullName = "Feature.Infrastructure.TestMyClass.MyTest(2)",
                                            },
                                            new TestCase()
                                            {
                                                Name = "MyTest(3)",
                                                FullName = "Feature.Infrastructure.TestMyClass.MyTest(3)",
                                            },
                                            new TestCase()
                                            {
                                                Name = "MyTest(4)",
                                                FullName = "Feature.Infrastructure.TestMyClass.MyTest(4)",
                                            },
                                        }
                                    },
                                }
                            }
                        }
                    }
                }
            };
        }

        private List<(string FullName, string Id)> GetFullNameAndIds(TestSuite suite)
        {
            List<(string FullName, string Id)> GetFullNameAndIdsFromCases(List<TestCase> testCases)
            {
                return testCases.Select(x => (x.FullName, x.Id)).ToList();
            }

            List<(string FullName, string Id)> GetFullNameAndIdsFromSuites(List<TestSuite> testSuites)
            {
                var results = new List<(string FullName, string Id)>();
                var r = testSuites.Select(GetFullNameAndIds).ToList();
                r.ForEach(x => results.AddRange(x));
                return results;
            }

            var result = new List<(string FullName, string Id)>() {(suite.FullName, suite.Id)};
            result.AddRange(GetFullNameAndIdsFromSuites(suite.TestSuites));
            result.AddRange(GetFullNameAndIdsFromCases(suite.TestCases));
            return result;
        }

        public Dictionary<TestPackage, TestSuite> GetTestPackageToTestSuiteMap
            (params TestAssemblyLocationHolder[] testAssembliesLocationHolder)
        {
            List<TestPackage> testPackages =
                testAssembliesLocationHolder.Select(x => new TestPackage(x.Location)).ToList();
            return testPackages.ToDictionary(x => x, GetTestSuite);
        }


        public Dictionary<string, (TestPackage TestPackage, string FullName)>
            GetIdToSchedulingParametersMap(Dictionary<TestPackage, TestSuite> packageToSuiteMap)
        {
            var result = new Dictionary<string, (TestPackage TestPackage, string Name)>();
            foreach (var packageAndSuite in packageToSuiteMap)
            {
                var fullNameAndIds = GetFullNameAndIds(packageAndSuite.Value);
                foreach (var fullNameAndId in fullNameAndIds)
                {
                    if (result.ContainsKey(fullNameAndId.Id)) throw new Exception($"The testable id {fullNameAndId.Id} is present more that one");
                    result.Add(fullNameAndId.Id, (packageAndSuite.Key, fullNameAndId.FullName));
                }
            }
            return result;
        }
    }
}