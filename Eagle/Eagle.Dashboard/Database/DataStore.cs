using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eagle.Dashboard.Models;
using Eagle.Dashboard.Services;

namespace Eagle.Dashboard.Database
{
    public class DataStore : IDataStore
    {
        public async Task CreateNode(NodeCreationParameters creationParameters)
        {
            //TODO error must be thrown if the same node name is already in use    
        }

        public async Task AddRequest(string requestId, string nodeName, string testId, DateTime requestTime, bool isRequestSuccessful)
        {
        }

        public async Task AddDiscoveredTests(string resultNodeName, List<TestSuite> resultTestSuites)
        {

        }

        public async Task AddTestResults(string resultNodeName, List<TestResult> resultTestResults)
        {

        }
    }
}