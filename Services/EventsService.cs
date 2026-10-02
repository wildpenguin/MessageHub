using MessageHub.Dtos;
using MessageHub.Repositories;

namespace MessageHub.Services;

public interface IEventsService
{
    Task<EventsResponse?> GetAsync(string id);
    Task<IReadOnlyList<EventsResponse>> ListAsync();
    Task<EventsResponse> CreateAsync(CreateEventsRequest request);
    Task <bool> DeleteAsync(string id);
}

public class EventsService : IEventsService
{
    private readonly IEventsRepository _events;

    public EventsService(IEventsRepository events) => _events = events;

    public async Task<EventsResponse> CreateAsync(CreateEventsRequest request)
    {
        var entity = request.ToEntity();
        var created = await _events.AddAsync(entity);

        return created.ToResponse();
    }

    public async Task<EventsResponse?> GetAsync(string id)
    {
        var events = await _events.GetByIdAsync(id);
        return events?.ToResponse();
    }

    public async Task<IReadOnlyList<EventsResponse>> ListAsync()
    {
        var events = await _events.GetAllAsync();
        return events.Select(e => e.ToResponse()).ToList();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _events.DeleteAsync(id);
    }
}