using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WonderTools.Eagle.Contract;
using Eagle.Dashboard.Models;

namespace Eagle.Dashboard.Services
{
    public interface IDataStore
    {
        Task CreateNode(NodeUpsertParameters creationParameters);
        Task AddRequest(string requestId, string nodeName, string testId, DateTime requestTime, bool isRequestSuccessful);
        Task AddDiscoveredTests(string resultNodeName, List<TestSuite> resultTestSuites);
        Task AddTestResults(string resultNodeName, List<TestResult> resultTestResults);
        Task<Dictionary<string, List<TestSuite>>> GetLatestTestSuites();
        Task<List<TestResult>> GetLatestTestResults();
        Task<string> GetUri(string nodeName);
        Task<List<Node>> GetNodes();
        Task<Node> UpdateNode(string nodeName, NodeUpsertParameters nodeUpsertParameters);
        Task<bool> DeleteNode(string nodeName);
    }
}