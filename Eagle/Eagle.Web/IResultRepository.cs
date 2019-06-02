using System.Collections.Generic;
using System.Threading.Tasks;
using Eagle.Web.Database;

namespace Eagle.Web
{
    public interface IResultRepository
    {
        Task<List<TestResult>> GetRecentTestResults(string id, int limit);
        Task<List<TestResult>> GetAllRecentTestResults();
        Task InsertTestResult(TestResult testResult);
    }
}