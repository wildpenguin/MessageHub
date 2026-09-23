namespace MessageHub.Models;

public class Groups
{
    public string? Id {get; set; }
    
    public string? GroupName { get; set; }
    
    public string? GroupType { get; set; }
    
    public string? GroupColorCode { get; set; }

    public List<string>? Members { get; set; }
}