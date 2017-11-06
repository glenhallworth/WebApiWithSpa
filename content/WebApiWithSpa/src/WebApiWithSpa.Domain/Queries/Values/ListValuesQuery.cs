using System.Threading.Tasks;

namespace WebApiWithSpa.Domain.Queries.Values
{
    public class ListValuesQuery : IQuery<string[]>
    {
        public Task<string[]> ExecuteAsync()
        {
            return Task.FromResult(new[] {"value1", "value2"});
        }
    }
}
