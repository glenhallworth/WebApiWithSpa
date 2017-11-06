using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using WebApiWithSpa.Api.Infrastructure.AutofacModules;
using WebApiWithSpa.Api.Infrastructure.Middleware;

namespace WebApiWithSpa.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = CreateLogger(env);
        }

        public IConfiguration Configuration { get; }

        private Logger CreateLogger(IHostingEnvironment env)
        {
            var logConfig = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Source", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", Configuration.GetSection("Application")["Environment"])
                .Enrich.WithProperty("ApplicationLayer", "WebAPI")
                .Enrich.WithProperty("ApplicationVersion",
                    TheAPI.Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion)
                .WriteTo.Console();

            var section = Configuration.GetSection("Logging");
            var seqUrl = section.GetValue<string>("SeqUrl");
            var seqApiKey = section.GetValue<string>("SeqApiKey");

            if (!string.IsNullOrEmpty(seqUrl))
                logConfig = string.IsNullOrEmpty(seqApiKey)
                    ? logConfig.WriteTo.Seq(seqUrl)
                    : logConfig.WriteTo.Seq(seqUrl, apiKey: seqApiKey);

            return logConfig.CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddCors();

            services.AddMvc()
                .AddJsonOptions(json =>
                {
                    json.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    json.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApiModule(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            ConfigureLogging(loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFileServerWithFallbackToIndex();
            app.UseAuthentication();

            app.UseWhen(
                context => context.Request.Path.StartsWithSegments(new PathString("/api")),
                whenApi =>
                {
                    var origins = Configuration.GetSection("Security")["AllowedCorsUrls"].Split(',');
                    whenApi.UseCors(policy =>
                    {
                        policy.WithOrigins(origins);
                        policy.AllowAnyMethod();
                        policy.AllowAnyHeader();
                    });
                }
            );
            
            app.UseMiddleware<SerilogMiddleware>();

            app.UseMvc();

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }

        private void ConfigureLogging(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
        }
    }
}
