using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eagle.Dashboard.Models;

namespace Eagle.Dashboard.Services
{
    public interface IDataStore
    {
        Task CreateNode(NodeCreationParameters creationParameters);
        Task AddRequest(string requestId, string nodeName, string testId, DateTime requestTime, bool isRequestSuccessful);
        Task AddDiscoveredTests(string resultNodeName, List<TestSuite> resultTestSuites);
        Task AddTestResults(string resultNodeName, List<TestResult> resultTestResults);
        Task<List<List<TestSuite>>> GetLatestTestSuites();
        Task<List<TestResult>> GetLatestTestResults();
    }
}