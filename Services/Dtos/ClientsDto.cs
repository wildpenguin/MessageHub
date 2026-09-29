using System.ComponentModel.DataAnnotations;
using MessageHub.Models;

namespace MessageHub.Dtos;

public record ClientResponse(
    string Id, 
    string Name,
    string Email,
    string Phone
);