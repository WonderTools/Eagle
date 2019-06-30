﻿using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using WonderTools.Eagle.Core;
using WonderTools.Eagle.Http;
using WonderTools.Eagle.Http.Contract;

namespace Eagle.TestNode2
{
    public static class EagleServiceExtension
    {
        public static void UseHttpEagleNUnit(this IApplicationBuilder app, string eagleEndpoint, params TestableAssembly[] testableAssemblies)
        {
            app.Use(async (context, next) =>
            {
                if (!IsEagleExecute(context, eagleEndpoint))
                {
                    await next.Invoke();
                    return;
                }

                await HandleEagleRequest(context, testableAssemblies);
            });
        }


        private static async Task HandleEagleRequest(HttpContext context, params TestableAssembly[] testableAssemblies)
        {
            var bodyAsText = await GetBody(context);
            TestTrigger testTrigger = JsonConvert.DeserializeObject<TestTrigger>(bodyAsText);

            var eagleEngine = new EagleEngine(testableAssemblies);
            var testSuites = eagleEngine.GetDiscoveredTestSuites();
            IResultHandler handler = new HttpRequestResultHandler(testSuites, testTrigger.NodeName, testTrigger.RequestId,
                testTrigger.CallBackUrl);
            var result = await eagleEngine.ExecuteTest(handler, testTrigger.Id);

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.OK;
            var report = new TestReport()
            {
                TestResults = result,
                TestSuites = testSuites,
                NodeName = testTrigger.NodeName,
                RequestId = testTrigger.RequestId,
            };
            await response.WriteAsync(JsonConvert.SerializeObject(report));
        }

        private static bool IsEagleExecute(HttpContext context, string eagleEndpoint)
        {
            var path = context.Request.Path.ToString().ToLower();
            if (path != eagleEndpoint) return false;
            if (context.Request.Method.ToLower() != "post") return false;
            return true;
        }


        private static async Task<string> GetBody(HttpContext context)
        {
            var request = context.Request;
            var body = request.Body;
            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableRewind();
            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;
            return bodyAsText;
        }
    }
}