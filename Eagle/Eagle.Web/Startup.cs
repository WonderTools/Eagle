using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Eagle.Web.Database;
using Eagle.Web.Service;
using Feature.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NUnit.Engine;

namespace Eagle.Web
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
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo  { Title = "Eagle Api", Version = "v1" });
            });

            services.AddTransient<IMyLogger, MyLogger>();
            services.AddSingleton<EagleEngine>();
            services.AddSingleton<HttpClient>();
            services.AddTransient<TestTreeService>();
            services.AddDbContext<ResultContext>(options => options.UseInMemoryDatabase("BoardGames"));
            services.AddTransient<IResultRepository, ResultRepository>();
            services.AddSingleton<IEagleEventListener, EagleEventListener>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, HttpClient client)
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

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();


            var serviceProvider = app.ApplicationServices;
            var eagleEngine = serviceProvider.GetService<EagleEngine>();
            var eventListener = serviceProvider.GetService<IEagleEventListener>();
            eagleEngine.Initialize(eventListener, typeof(TestClass));
            TriggerProcess(app, client);
        }

        private static void TriggerProcess(IApplicationBuilder app, HttpClient httpClient)
        {
            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            var addresses = serverAddressesFeature.Addresses;
            if (addresses.Count == 0) throw new Exception("No listening addresses available");
            var address = addresses.ElementAt(0);

            var timer = new System.Threading.Timer(PostToProcess, null, 5000, 5000);
            void PostToProcess(object o)
            {
                var url = address + "/api/process";
                httpClient.PostAsync(url, new StringContent(""));
            }
        }
    }

    public class MyLogger : IMyLogger
    {
        private readonly ILogger<MyLogger> _logger;

        public MyLogger(ILogger<MyLogger> logger)
        {
            _logger = logger;
        }

        public void Log(string log)
        {
            _logger.LogError(log);
        }
    }

    public class EagleEventListener : IEagleEventListener
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EagleEventListener(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task TestCompleted(string id, string result, DateTime startingTime, DateTime finishingTime, int duration)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IResultRepository>();
                await repo.InsertTestResult(new TestResult()
                {
                    Id = id,
                    StartingTime = startingTime,
                    FinishingTime = finishingTime,
                    Result = result,
                    DurationInMs = duration,
                });
            }
        }
    }
}
