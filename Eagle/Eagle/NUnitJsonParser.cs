using System.Collections.Generic;

namespace Eagle
{
    public class NUnitJsonParser
    {
        public TestSuite GetTestSuiteFromDiscoveryJson(string json)
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
    }
}