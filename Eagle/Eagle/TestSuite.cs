using System.Collections.Generic;

namespace Eagle
{
    //Should this be renamed to DiscoveryTestSuite
    public class TestSuite : Testable
    {
        public TestSuite()
        {
            TestSuites = new List<TestSuite>();
            TestCases = new List<TestCase>();
        }

        public List<TestSuite> TestSuites { get; set; }

        public List<TestCase> TestCases { get; set; }
    }
}