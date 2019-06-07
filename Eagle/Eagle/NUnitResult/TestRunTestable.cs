using System;
using System.Collections.Generic;

namespace Eagle.NUnitResult
{
  

    public class RunTestSuite 
    {
        public RunTestSuite()
        {
            TestSuites = new List<RunTestSuite>();
            TestCases = new List<RunTestCase>();
        }

        public List<RunTestSuite> TestSuites { get; set; }

        public List<RunTestCase> TestCases { get; set; }

        public string FullName { get; set; }

    }

    public class RunTestCase 
    {
        public string FullName { get; set; }

        public string Result { get; set; }
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public string Duration { get; set; }

    }
}
