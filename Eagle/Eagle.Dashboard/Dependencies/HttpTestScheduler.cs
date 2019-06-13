using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Eagle.Dashboard.Services;

namespace Eagle.Dashboard.Dependencies
{
    public class HttpTestScheduler: ITestScheduler
    {
        public Task Schedule(string nodeName, string uri, string testId, string requestId)
        {
            HttpClient httpClient= new HttpClient();
            throw new NotImplementedException();
        }
    }

    public class ExecuteParameters
    {
        //TODO copied code. Remove Duplication
        public string NodeName { get; set; }
        public string Id { get; set; }
        public string CallBackUrl { get; set; }
        public string RequestId { get; set; }
    }
}
