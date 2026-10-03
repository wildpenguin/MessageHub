using MessageHub.Dtos;
using MessageHub.Models;
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
    private readonly IGroupsService _groups;
    private readonly INotificationsRepository _notifications;

    public EventsService(
        IEventsRepository events,
        IGroupsService groups,
        INotificationsRepository notifications
    ) {
        _events = events;
        _groups = groups;
        _notifications = notifications;
    }

    public async Task<EventsResponse> CreateAsync(CreateEventsRequest request)
    {
        var entity = request.ToEntity();
        // Use the returned copy: only it has the Firestore id the notifications need.
        var created = await _events.AddAsync(entity);

        await NotifyGroupAsync(created);

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

    private async Task NotifyGroupAsync(Events eventMessage)
    {
        List<Notifications> createNotifications = [];

        var clients = await _groups.GetGroupClientsAsync(eventMessage.Groups);
        foreach (var client in clients)
        {
            var (channel, recipient) =
                !string.IsNullOrWhiteSpace(client.Phone) ? (NotificationChannel.Sms, client.Phone) :
                !string.IsNullOrWhiteSpace(client.Email) ? (NotificationChannel.Email, client.Email) :
                (default, null);

            if (recipient is null) continue;

            createNotifications.Add(new Notifications()
            {
                EventId = eventMessage.Id,
                ClientId = client.Id,
                Channel = channel,
                Recipient = recipient,
                Subject = eventMessage.EventTitle,
                Body = eventMessage.EventText,
            });
        }

        await _notifications.AddRangeAsync(createNotifications);
    }
}