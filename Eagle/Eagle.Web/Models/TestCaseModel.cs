using System;

namespace Eagle.Web.Models
{
    public class TestCaseModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public DateTime? ExecutionStartTime { get; set; }
        public DateTime? ExecutionEndTime { get; set; }
    }
}