using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Eagle.Dashboard.Services;
using Newtonsoft.Json;

namespace Eagle.Dashboard.Dependencies
{
    public class HttpTestScheduler: ITestScheduler
    {
        public async Task Schedule(string nodeName, string uri, string testId, string requestId, string callBackUri)
        {
            HttpClient httpClient= new HttpClient();
            var parameters = new ExecuteParameters
                {NodeName = nodeName, Id = testId, CallBackUrl = callBackUri, RequestId = requestId};
            await httpClient.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(parameters)));

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
