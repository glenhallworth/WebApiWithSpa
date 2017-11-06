using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiWithSpa.Api.Infrastructure.Dispatchers;
using WebApiWithSpa.Domain.Commands.Values;
using WebApiWithSpa.Domain.Queries.Values;

namespace WebApiWithSpa.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly GetValueQuery _getValueQuery;
        private readonly ListValuesQuery _listValuesQuery;
        private readonly CommandDispatcher _commandDispatcher;

        public ValuesController(GetValueQuery getValueQuery, ListValuesQuery listValuesQuery, CommandDispatcher commandDispatcher)
        {
            _getValueQuery = getValueQuery;
            _listValuesQuery = listValuesQuery;
            _commandDispatcher = commandDispatcher;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return await _listValuesQuery.ExecuteAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return await _getValueQuery.ExecuteAsync(id);
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]string value)
        {
            var createValueCommand = new CreateValueCommand()
            {
                Value = value
            };
            await _commandDispatcher.DispatchAsync(createValueCommand);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]string value)
        {
            var updateValueCommand = new UpdateValueCommand()
            {
                Id = id,
                Value = value
            };
            await _commandDispatcher.DispatchAsync(updateValueCommand);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var deleteValueCommand = new DeleteValueCommand()
            {
                Id = id
            };
            await _commandDispatcher.DispatchAsync(deleteValueCommand);
        }
    }
}
