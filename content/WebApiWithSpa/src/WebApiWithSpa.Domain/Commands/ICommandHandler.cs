using System.Threading.Tasks;

namespace WebApiWithSpa.Domain.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task ExecuteAsync(T command);
    }
}
