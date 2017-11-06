using System.Threading.Tasks;

namespace WebApiWithSpa.Domain.Queries
{
    public interface IQuery<T>
    {
        Task<T> ExecuteAsync();
    }

    public interface IQuery<TArgs, TResult>
    {
        Task<TResult> ExecuteAsync(TArgs args);
    }
}
