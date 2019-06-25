using System.Collections.Generic;

namespace Eagle.Contract
{
    //Should this be renamed to DiscoveryTestSuite
    public class TestSuite
    {
        public TestSuite()
        {
            TestSuites = new List<TestSuite>();
            TestCases = new List<TestCase>();
        }

        public string Id { get; set; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public List<TestSuite> TestSuites { get; set; }

        public List<TestCase> TestCases { get; set; }
    }
}