using System.Threading.Tasks;
using Autofac;
using Serilog;
using WebApiWithSpa.Domain.Commands;

namespace WebApiWithSpa.Api.Infrastructure.Dispatchers
{
    public class CommandDispatcher
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger _logger;

        public CommandDispatcher(ILifetimeScope scope, ILogger logger)
        {
            _scope = scope;
            _logger = logger.ForContext<CommandDispatcher>();
        }

        public Task DispatchAsync<T>(T command) where T : ICommand
        {
            var handler = _scope.Resolve<ICommandHandler<T>>();

            _logger.ForContext("CommandData", command, true)
                .Information("Executing {Command} using {Handler}", command.GetType().Name, handler.GetType().Name);

            return handler.ExecuteAsync(command);
        }
    }
}
