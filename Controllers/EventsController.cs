using Microsoft.AspNetCore.Mvc;

using MessageHub.Dtos;
using MessageHub.Services;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventsService _service;

    public EventsController(EventsService service) => _service = service;

    [HttpGet("{id}")]
    public async Task<ActionResult<EventsResponse>> Get(string id)
    {
        var events = await _service.GetAsync(id);

        return events is null ? NotFound() : Ok(events);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EventsResponse>>> List()
    {
        return Ok(await _service.ListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<EventsResponse>> Create(CreateEventsRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(string id)
    {
        return await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}