using AppLogic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class EventTypeGroupsController(IEventLogic eventLogic, ILogger<EventTypeGroupsController> logger) : ControllerBase
{
    private readonly IEventLogic _eventLogic = eventLogic;
    private readonly ILogger<EventTypeGroupsController> _logger = logger;

    /// <summary>
    /// Get all event type groups
    /// </summary>
    /// <returns>List of event type groups</returns>
    [HttpGet]
    public async Task<IActionResult> GetEventTypeGroups()
    {
        try
        {
            _logger.LogInformation("Retrieving all event type groups");
            var eventTypeGroups = await _eventLogic.GetEventTypeGroupsAsync().ConfigureAwait(false);
            return Ok(eventTypeGroups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve event type groups");
            return StatusCode(500, "An error occurred while retrieving event type groups");
        }
    }

    /// <summary>
    /// Get a specific event type group by ID
    /// </summary>
    /// <param name="id">Event type group ID</param>
    /// <returns>Event type group details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventTypeGroup(string id)
    {
        try
        {
            _logger.LogInformation("Retrieving event type group with ID: {Id}", id);
            var eventTypeGroup = await _eventLogic.GetEventTypeGroupByIdAsync(id).ConfigureAwait(false);
            
            if (eventTypeGroup == null)
            {
                return NotFound($"Event type group with ID '{id}' not found");
            }

            return Ok(eventTypeGroup);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve event type group with ID: {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the event type group");
        }
    }
}
