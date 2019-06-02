using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eagle.Web.Models
{
    public class TestSuiteModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public List<TestSuiteModel> TestSuites { get; set; }
        public List<TestCaseModel> TestCases { get; set; }
    }
}
