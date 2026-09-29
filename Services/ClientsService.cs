namespace MessageHub.Services;

using MessageHub.Dtos;
using MessageHub.Repositories;

public interface IClientsService
{
    Task<ClientResponse?> GetAsync(string id);
    Task<IReadOnlyList<ClientResponse>> ListAsync();
    Task <ClientResponse> CreateAsync(CreateClientRequest request);
    Task <bool> UpdateAsync(string id, UpdateProductRequest request);
    Task<bool> DeleteAsync(string id);
}

public class ClientsService: IClientsService
{
    private readonly IClientsRepository _clients;

    public ClientsService(IClientsRepository clients) => _clients = clients;

    public async Task<ClientsResponse?> GetAsync(string id)
    {
        var client = await _clients.GetByIdAsync(id);
        return client?.ToResponse();
    } 
}