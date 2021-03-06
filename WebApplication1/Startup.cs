using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApplication1
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
            //maps to custom configuration section
            services
                .Configure<ServiceSettings>(Configuration.GetSection(nameof(ServiceSettings)));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1", Version = "v1" });
            });

            ////moved this into the typed class
            //services.AddHttpClient<WeatherClient>(o =>
            //{
            //   var serviceSettings = Configuration.GetSection(nameof(ServiceSettings))
            //                                 .Get<ServiceSettings>();
            //   o.BaseAddress = new Uri(serviceSettings.WeatherBitHost);
            //   o.DefaultRequestHeaders.Add("Accept", "application/json");
            //   o.DefaultRequestHeaders.Add("Connection", "keep-alive");

            //});

            services.AddHttpClient<WeatherClient>()
                .AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(3,
                        //exponential backoff, longer each time i.e wait 2, 4, 16 seconds
                        retyAttempt => TimeSpan.FromSeconds(Math.Pow(2, retyAttempt))))
                .AddTransientHttpErrorPolicy(p =>
                    //circuitbraker give fast responses without retry until time passed
                    p.CircuitBreakerAsync(2, TimeSpan.FromSeconds(10)));

            services.AddHealthChecks()
                .AddCheck<ExternalEndPointHealthCheck>("WeatherEndPoint");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
