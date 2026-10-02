using MessageHub.Dtos;
using MessageHub.Repositories;

namespace MessageHub.Services;

public interface IGroupsService
{
    Task<GroupResponse?> GetAsync(string id);
    Task<IReadOnlyList<GroupResponse>> ListAsync();
    Task<GroupResponse> CreateAsync(CreateGroupRequest request);
    Task<bool> UpdateAsync(string id, UpdateGroupRequest request);
    Task<bool> DeleteAsync(string id);
}

public class GroupsService : IGroupsService
{
    private readonly IGroupsRepository _groups;

    public GroupsService(IGroupsRepository groups) => _groups = groups;

    public async Task<GroupResponse?> GetAsync(string id)
    {
        var group = await _groups.GetByIdAsync(id);
        return group?.ToResponse();
    }

    public async Task<IReadOnlyList<GroupResponse>> ListAsync()
    {
        var groups = await _groups.GetAllAsync();
        return groups.Select(g => g.ToResponse()).ToList();
    }

    public async Task<GroupResponse> CreateAsync(CreateGroupRequest request)
    {
        var entity = request.ToEntity();

        var created = await _groups.AddAsync(entity);
        return created.ToResponse();
    }

    public async Task<bool> UpdateAsync(string id, UpdateGroupRequest request)
    {
        var existing = await _groups.GetByIdAsync(id);
        if (existing is null) return false;

        request.ApplyTo(existing);
        return await _groups.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(string id) =>
        await _groups.DeleteAsync(id);
}