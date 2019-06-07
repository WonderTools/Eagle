using System;

namespace Eagle
{
    public class ResultTestCase 
    {
        public string FullName { get; set; }

        public string Result { get; set; }
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public string Duration { get; set; }

    }
}