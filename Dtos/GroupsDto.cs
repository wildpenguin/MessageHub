using System.ComponentModel.DataAnnotations;
using MessageHub.Models;

namespace MessageHub.Dtos;

public record GroupResponse(
    string Id,
    string GroupName,
    string? GroupType,
    string? GroupColorCode,
    List<string> Members
);

public record CreateGroupRequest(
    [Required, StringLength(100)] string GroupName,
    [StringLength(50)] string? GroupType,
    [RegularExpression("^#[0-9A-Fa-f]{6}$")] string? GroupColorCode,
    List<string>? Members
);

public record UpdateGroupRequest(
    [Required, StringLength(100)] string GroupName,
    [StringLength(50)] string? GroupType,
    [RegularExpression("^#[0-9A-Fa-f]{6}$")] string? GroupColorCode,
    List<string>? Members
);

public static class GroupsMappings
{
    public static GroupResponse ToResponse(this Groups g) => new(
        g.Id,
        g.GroupName,
        g.GroupType,
        g.GroupColorCode,
        g.Members
    );

    public static Groups ToEntity(this CreateGroupRequest r) => new()
    {
        GroupName = r.GroupName,
        GroupType = r.GroupType,
        GroupColorCode = r.GroupColorCode,
        Members = r.Members ?? new()
    };

    public static void ApplyTo(this UpdateGroupRequest r, Groups g)
    {
        g.GroupName = r.GroupName;
        g.GroupType = r.GroupType;
        g.GroupColorCode = r.GroupColorCode;
        g.Members = r.Members ?? new();
    }
}