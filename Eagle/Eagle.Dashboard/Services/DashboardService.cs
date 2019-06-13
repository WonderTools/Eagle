using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Dashboard.Models;

namespace Eagle.Dashboard.Services
{
    public class DashboardService
    {
        private readonly IDataStore _dataStore;
        private readonly ITestScheduler _testScheduler;

        public DashboardService(IDataStore dataStore, ITestScheduler testScheduler)
        {
            _dataStore = dataStore;
            _testScheduler = testScheduler;
        }

        public async Task CreateNode(NodeCreationParameters creationParameters)
        {
            //TODO : Validate model
            await _dataStore.CreateNode(creationParameters);
            var requestTime = DateTime.Now;
            var requestId = GenerateResultId(requestTime);
            var isRequestSuccessful = await ScheduleTest(creationParameters, requestId);
            await _dataStore.AddRequest(requestId, creationParameters.NodeName, string.Empty, requestTime, isRequestSuccessful);
        }

        private async Task<bool> ScheduleTest(NodeCreationParameters creationParameters, string requestId)
        {
            try
            {
                await _testScheduler.Schedule(creationParameters.NodeName, creationParameters.Uri, string.Empty, requestId, String.Empty);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string GenerateResultId(DateTime requestTime)
        {
            return requestTime.ToString("yyyy-MM-dd-HH:mm:ss.fff") + new Guid();
        }
    }
}
