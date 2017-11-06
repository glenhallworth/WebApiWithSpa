using System.Threading.Tasks;

namespace WebApiWithSpa.Domain.Commands.Values
{
    public class DeleteValueCommandHandler : ICommandHandler<DeleteValueCommand>
    {
        public Task ExecuteAsync(DeleteValueCommand command)
        {
            return Task.CompletedTask;
        }
    }
}
