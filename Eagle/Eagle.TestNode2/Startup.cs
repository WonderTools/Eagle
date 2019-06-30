using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using WonderTools.Eagle;
using WonderTools.Eagle.Core;
using WonderTools.Eagle.Http;
using WonderTools.Eagle.Http.Contract;

namespace Eagle.TestNode2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info() { Title = "Node2 Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.Use(async (context, next) =>
            {
                if (!IsEagleExecute(context))
                {
                    await next.Invoke();
                    return;
                }

                await HandleEagleRequest(context);
            });


            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private static async Task HandleEagleRequest(HttpContext context)
        {
            var bodyAsText = await GetBody(context);
            TestTrigger testTrigger = JsonConvert.DeserializeObject<TestTrigger>(bodyAsText);

            var eagleEngine = new EagleEngine(typeof(AzureResponseCodeTests));
            var testSuites = eagleEngine.GetDiscoveredTestSuites();
            IResultHandler handler = new HttpRequestResultHandler(testSuites, testTrigger.NodeName, testTrigger.RequestId,
                testTrigger.CallBackUrl);
            var result = await eagleEngine.ExecuteTest(handler, testTrigger.Id);

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int) HttpStatusCode.OK;
            var report = new TestReport()
            {
                TestResults = result,
                TestSuites = testSuites,
                NodeName = testTrigger.NodeName,
                RequestId = testTrigger.RequestId,
            };
            await response.WriteAsync(JsonConvert.SerializeObject(report));
        }

        private bool IsEagleExecute(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();
            if (path != "/api/tests/execute") return false;
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
