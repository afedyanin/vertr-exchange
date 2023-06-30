using Microsoft.AspNetCore.Mvc;
using Vertr.OrderMatching.Api.Disruptor;

namespace Vertr.OrderMatching.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDisruptorService _disruptorService;

        public ValuesController(IDisruptorService disruptorService)
        {
            _disruptorService = disruptorService;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            _disruptorService.PublishPing("From GET");

            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            _disruptorService.PublishPing($"From GET with Id={id}");

            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            _disruptorService.PublishPing($"From POST");
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            _disruptorService.PublishPing($"From PUT with Id={id}");
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _disruptorService.PublishPing($"From DELETE with Id={id}");
        }
    }
}
