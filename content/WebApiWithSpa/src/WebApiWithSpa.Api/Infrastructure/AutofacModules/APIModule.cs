using Autofac;
using AutofacSerilogIntegration;
using Microsoft.Extensions.Configuration;
using Serilog;
using WebApiWithSpa.Api.Infrastructure.Dispatchers;
using WebApiWithSpa.Domain.Infrastructure.AutofacModules;

namespace WebApiWithSpa.Api.Infrastructure.AutofacModules
{
    public class ApiModule : Module
    {
        private readonly IConfiguration _configuration;

        public ApiModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterLogger(Log.Logger);

            builder.RegisterType<CommandDispatcher>();

            builder.RegisterModule<DomainModule>();
        }
    }
}
