using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WonderTools.Eagle.Communication.Contract;
using Eagle.Dashboard.Services;
using Newtonsoft.Json;

namespace Eagle.Dashboard.Dependencies
{
    public class HttpTestScheduler: ITestScheduler
    {
        public async Task Schedule(string nodeName, string uri, string testId, string requestId, string callBackUri)
        {
            HttpClient httpClient= new HttpClient();
            var parameters = new TestTrigger
                {NodeName = nodeName, Id = testId, CallBackUrl = callBackUri, RequestId = requestId};

            var stringContent = new StringContent(JsonConvert.SerializeObject(parameters));
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(uri, stringContent);
        }
    }
}
