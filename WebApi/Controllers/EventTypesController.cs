using AppLogic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class EventTypesController(IEventLogic eventLogic, ILogger<EventTypesController> logger) : ControllerBase
{
    private readonly IEventLogic _eventLogic = eventLogic;
    private readonly ILogger<EventTypesController> _logger = logger;

    /// <summary>
    /// Get all event types
    /// </summary>
    /// <returns>List of event types</returns>
    [HttpGet]
    public async Task<IActionResult> GetEventTypes()
    {
        try
        {
            _logger.LogInformation("Retrieving all event types");
            var eventTypes = await _eventLogic.GetEventTypesAsync().ConfigureAwait(false);
            return Ok(eventTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve event types");
            return StatusCode(500, "An error occurred while retrieving event types");
        }
    }

    /// <summary>
    /// Get a specific event type by ID
    /// </summary>
    /// <param name="id">Event type ID</param>
    /// <returns>Event type details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventType(string id)
    {
        try
        {
            _logger.LogInformation("Retrieving event type with ID: {Id}", id);
            var eventType = await _eventLogic.GetEventTypeByIdAsync(id).ConfigureAwait(false);
            
            if (eventType == null)
            {
                return NotFound($"Event type with ID '{id}' not found");
            }

            return Ok(eventType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve event type with ID: {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the event type");
        }
    }
}
