using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cyf.MicroService.AggregateService.Services;
using Cyf.MicroService.Core.HttpClientConsul;
using Cyf.MicroService.Core.HttpClientPolly;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cyf.MicroService.Test
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
            // 1、自定义异常处理(用缓存处理)
            var fallbackResponse = new HttpResponseMessage
            {
                Content = new StringContent("系统正繁忙，请稍后重试"),// 内容，自定义内容
                StatusCode = HttpStatusCode.GatewayTimeout // 504
            };

            // 1.2 封装之后的调用PollyHttpClient
            //services.AddPollyHttpClient("consul", options =>
            //{
            //    options.TimeoutTime = 1; // 1、超时时间
            //    options.RetryCount = 3;// 2、重试次数
            //    options.CircuitBreakerOpenFallCount = 2;// 3、熔断器开启(多少次失败开启)
            //    options.CircuitBreakerDownTime = 100;// 4、熔断器开启时间
            //    options.httpResponseMessage = fallbackResponse;// 5、降级处理
            //});

            //// 1、注册Consul服务
            //services.AddHttpClientConsul<ConsulHttpClient>();
            //// 2、注册team服务
            //services.AddSingleton<ITeamServiceClient, HttpTeamServiceClient>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
