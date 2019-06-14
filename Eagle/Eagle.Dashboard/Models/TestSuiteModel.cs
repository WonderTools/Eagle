using System;
using System.Collections.Generic;

namespace Eagle.Dashboard.Models
{
    public class TestSuiteModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public DateTime? FinishingTime { get; set; }
        public DateTime? StartingTime { get; set; }

        public List<TestSuiteModel> TestSuites { get; set; } = new List<TestSuiteModel>();
        public List<TestCaseModel> TestCases { get; set; } = new List<TestCaseModel>();
    }
}