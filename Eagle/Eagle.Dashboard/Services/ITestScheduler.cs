using System.Threading.Tasks;

namespace Eagle.Dashboard.Services
{
    public interface ITestScheduler
    {
        Task Schedule(string nodeName, string uri, string testId, string requestId, string callBackUri);
    }
}