using System.Threading.Tasks;

namespace WebApiWithSpa.Domain.Commands.Values
{
    public class UpdateValueCommandHandler: ICommandHandler<UpdateValueCommand>
    {
        public Task ExecuteAsync(UpdateValueCommand command)
        {
            return Task.CompletedTask;
        }
    }
}
