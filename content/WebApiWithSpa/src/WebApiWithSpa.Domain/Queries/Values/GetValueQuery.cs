using System.Threading.Tasks;

namespace WebApiWithSpa.Domain.Queries.Values
{
    public class GetValueQuery : IQuery<int, string>
    {
        public Task<string> ExecuteAsync(int args)
        {
            return Task.FromResult("value");
        }
    }
}
