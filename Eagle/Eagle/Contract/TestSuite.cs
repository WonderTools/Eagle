﻿using System.Collections.Generic;

namespace Eagle.Contract
{
    //Should this be renamed to DiscoveryTestSuite
    public class TestSuite1
    {
        public TestSuite1()
        {
            TestSuites = new List<TestSuite1>();
            TestCases = new List<TestCase1>();
        }

        public string Id { get; set; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public List<TestSuite1> TestSuites { get; set; }

        public List<TestCase1> TestCases { get; set; }
    }
}