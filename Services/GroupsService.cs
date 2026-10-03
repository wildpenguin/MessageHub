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
    Task<IReadOnlyList<ClientResponse>> GetGroupClientsAsync(IEnumerable<string> groupIds);

}

public class GroupsService : IGroupsService
{
    private readonly IGroupsRepository _groups;
    private readonly IClientsService _clients;

    public GroupsService(IGroupsRepository groups, IClientsService clients)
    {
        _groups = groups;
        _clients = clients;
    } 

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
    
    public async Task<IReadOnlyList<ClientResponse>> GetGroupClientsAsync(IEnumerable<string> groupIds)
    {
        var memberIds = new HashSet<string>();

        foreach (var groupId in groupIds.Distinct())
        {
            var group = await _groups.GetByIdAsync(groupId);
            if (group is null) continue;

            memberIds.UnionWith(group.Members);
        }

        var clientsList = new List<ClientResponse>();

        foreach (var m in memberIds) {
            var getClient = await _clients.GetAsync(m);
            if (getClient is null) continue;

            clientsList.Add(getClient);
        }
        return clientsList;
    } 
}