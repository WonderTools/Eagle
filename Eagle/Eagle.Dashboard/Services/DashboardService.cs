using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Dashboard.Models;
using Microsoft.AspNetCore.Hosting.Server.Features;

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
            //TODO : The Uri is Currently hardcoded, but this needs to be injected. It also needs to be configured
            //TODO : This needs to be injected as we could switch to messaging using RabbitMQ
            var isRequestSuccessful = await ScheduleTest(creationParameters, requestId, "https://localhost:6501/api/results");
            await _dataStore.AddRequest(requestId, creationParameters.NodeName, string.Empty, requestTime, isRequestSuccessful);
        }

        private async Task<bool> ScheduleTest(NodeCreationParameters creationParameters, string requestId, string callBackUri)
        {
            try
            {
                await _testScheduler.Schedule(creationParameters.NodeName, creationParameters.Uri, string.Empty, requestId, callBackUri);
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
