using System;
using System.Threading.Tasks;
using Eagle.Dashboard.Models;

namespace Eagle.Dashboard.Services
{
    public interface IDataStore
    {
        Task CreateNode(NodeCreationParameters creationParameters);
        Task AddRequest(string requestId, string nodeName, string testId, DateTime requestTime, bool isRequestSuccessful);
    }
}