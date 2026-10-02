using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MessageHub.Dtos;
using MessageHub.Services;

[ApiController]
[Authorize]
[Route("api/groups")]
public class GroupsController : ControllerBase
{
    private readonly IGroupsService _service;

    public GroupsController(IGroupsService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GroupResponse>>> List()
        => Ok(await _service.ListAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupResponse>> Get(string id)
    {
        var group = await _service.GetAsync(id);
        return group is null ? NotFound() : Ok(group);
    }

    [HttpPost]
    public async Task<ActionResult<GroupResponse>> Create(CreateGroupRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateGroupRequest request)
        => await _service.UpdateAsync(id, request) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
        => await _service.DeleteAsync(id) ? NoContent() : NotFound();
}