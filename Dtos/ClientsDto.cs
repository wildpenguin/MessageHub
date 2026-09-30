using System.ComponentModel.DataAnnotations;
using MessageHub.Models;

namespace MessageHub.Dtos;

public record ClientResponse(
    string Id, 
    string Name,
    string ?Email,
    string ?Phone
);

public record CreateClientRequest(
    [Required, StringLength(100)] string Name,
    [Required, EmailAddress] string Email,
    [Phone] string Phone
);

public record UpdateClientRequest(
    [Required, StringLength(100)] string Name,
    [Required, EmailAddress] string Email,
    [Phone] string Phone
);

public static class ClientsMappings
{
    public static ClientResponse ToResponse(this Clients c) => new(
        c.Id, 
        c.Name,
        c.Email,
        c.Phone
    );

    public static Clients ToEntity(this CreateClientRequest r) => new ()
    {
        Name = r.Name,
        Email = r.Email,
        Phone = r.Phone
    };

    public static void ApplyTo(this UpdateClientRequest r, Clients c)
    {
        c.Name = r.Name;
        c.Email = r.Email;
        c.Phone = r.Phone;
    }
}