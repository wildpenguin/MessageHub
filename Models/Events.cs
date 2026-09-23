namespace MessageHub.Models;

public class Events
{
    public string? Id { get; set; }
    public string Type { get; set; } = ""; // emergency, info
    public string Title { get; set; } = "";
    public string Text { get; set; } = "";
}