using System;
using System.Collections.Generic;

namespace PlayGround
{
    public class TestRunTestable
    {
        public abstract class Testable
        {
            public string Id => "id" + FullName;

            public string FullName { get; set; }

            public string Result { get; set; }
            public DateTimeOffset StartTime { get; set; }

            public DateTimeOffset EndTime { get; set; }

            public string Duration { get; set; }
        }
    }

    public class RunTestSuite : TestRunTestable
    {
        public RunTestSuite()
        {
            TestSuites = new List<RunTestSuite>();
            TestCases = new List<RunTestCase>();
        }

        public List<RunTestSuite> TestSuites { get; set; }

        public List<RunTestCase> TestCases { get; set; }
    }

    public class RunTestCase : TestRunTestable
    {
        public void Log(string log)
        {
        }
    }
}
