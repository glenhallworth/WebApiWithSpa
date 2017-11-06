using System.Threading.Tasks;

namespace WebApiWithSpa.Domain.Commands.Values
{
    public class CreateValueCommandHandler : ICommandHandler<CreateValueCommand>
    {
        public Task ExecuteAsync(CreateValueCommand command)
        {
            return Task.CompletedTask;
        }
    }
}
