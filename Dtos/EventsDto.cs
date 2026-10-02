using MessageHub.Models;
using System.ComponentModel.DataAnnotations;

namespace MessageHub.Dtos;

public record EventsResponse(
    string Id,
    string EventType,
    string EventTitle,
    string EventText,
    List<string> Groups,
    DateTime CreatedAt
);

public record CreateEventsRequest(
    [Required, StringLength(100)] string EventType,
    [Required, StringLength(100)] string EventTitle,
    [Required, StringLength(200)] string EventText,
    [Required] List<string> Groups
);

public static class EventsMappings
{
    public static EventsResponse ToResponse(this Events e) => new(
        e.Id,
        e.EventType,
        e.EventTitle,
        e.EventText,
        e.Groups,
        e.CreatedAt.ToDateTime()
    );

    public static Events ToEntity(this CreateEventsRequest r) => new()
    {
        EventType = r.EventType,
        EventTitle = r.EventTitle,
        EventText = r.EventText,
        Groups = r.Groups ?? []
    };

}