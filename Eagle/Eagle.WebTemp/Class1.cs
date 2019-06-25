using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Eagle.Contract;
using Newtonsoft.Json;

namespace Eagle.WebTemp
{
    public class Class1
    {
    }

    public class TestTrigger
    {
        public string NodeName { get; set; }
        public string Id { get; set; }
        public string CallBackUrl { get; set; }
        public string RequestId { get; set; }
    }

    public class HttpRequestResultHandler : IResultHandler
    {
        //TODO The exception should not be always swallowed. This should be swallowed only in development mode set by a configuration in appsettings.json 
        public async Task OnTestCompletion(string listenerUri, MyResult result)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var serializedResult = JsonConvert.SerializeObject(result);
                    var content = new StringContent(serializedResult);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await httpClient.PostAsync(listenerUri, content);
                }
            }
            catch (Exception e)
            {

            }

        }
    }


}
