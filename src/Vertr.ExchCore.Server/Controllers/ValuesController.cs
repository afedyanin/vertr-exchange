namespace Vertr.ExchCore.Server.Controllers;

using Microsoft.AspNetCore.Mvc;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Enums;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly IOrderManagementApi _api;

    public ValuesController(IOrderManagementApi api)
    {
        _api = api;
    }
    // GET: api/<OrdersController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        _api.PlaceNewOrder(1, 1, 1, 1, OrderAction.ASK, OrderType.GTC, 1, 1);

        return new string[] { "value1", "value2" };
    }

    // GET api/<OrdersController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<OrdersController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<OrdersController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<OrdersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
