
using MessageHub.Dtos;
using MessageHub.Repositories;
namespace MessageHub.Services;

public interface IClientsService
{
    Task<ClientResponse?> GetAsync(string id);
    Task<IReadOnlyList<ClientResponse>> ListAsync(string? name);
    Task <ClientResponse> CreateAsync(CreateClientRequest request);
    Task <bool> UpdateAsync(string id, UpdateClientRequest request);
    Task<bool> DeleteAsync(string id);
}

public class ClientsService: IClientsService
{
    private readonly IClientsRepository _clients;

    public ClientsService(IClientsRepository clients) => _clients = clients;

    public async Task<ClientResponse?> GetAsync(string id)
    {
        var client = await _clients.GetByIdAsync(id);
        return client?.ToResponse();
    }

    public async Task<IReadOnlyList<ClientResponse>> ListAsync(string? name)
    {
        var clients = string.IsNullOrWhiteSpace(name)
            ? await _clients.GetAllAsync()
            : await _clients.GetByNameAsync(name);

        return clients.Select(c => c.ToResponse()).ToList();
    }

    public async Task<ClientResponse> CreateAsync(CreateClientRequest request)
    {
        var entity = request.ToEntity();
        
        var created = await _clients.AddAsync(entity);
        return created.ToResponse();
    }

    public async Task<bool> UpdateAsync(string id, UpdateClientRequest request)
    {
        var existing = await _clients.GetByIdAsync(id);
        if (existing is null) return false;

        request.ApplyTo(existing);
        return await _clients.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(string id) => 
       await _clients.DeleteAsync(id);
    
}