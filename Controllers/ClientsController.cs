using Microsoft.AspNetCore.Mvc;

using MessageHub.Dtos;
using MessageHub.Services;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _service;

    public ClientsController(IClientsService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClientResponse>>> List(
        [FromQuery] string? name) => Ok(await _service.ListAsync(name));
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientResponse>> Get(string id)
    {
        var client = await _service.GetAsync(id);
        return client is null ? NotFound(): Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<ClientResponse>> Create(CreateClientRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateClientRequest request) 
        => await _service.UpdateAsync(id, request) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound(); 
}