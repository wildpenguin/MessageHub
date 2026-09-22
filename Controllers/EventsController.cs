using Microsoft.AspNetCore.Mvc;

namespace MessageHub.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Event>> GetEvents()
    {
        return new Event{
            "Prince Eduard School",
            "123 ColonelBy drive",
            "info" // notification type
        };
    }
}