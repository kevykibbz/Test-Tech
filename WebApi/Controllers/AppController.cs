using AppLogic;
using Microsoft.AspNetCore.Mvc;
using ServiceModel;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class AppController(ILogLogic logLogic, ILogger<AppController> logger) : ControllerBase
{
    private readonly ILogLogic _logLogic = logLogic;
    private readonly ILogger<AppController> _logger = logger;

    /// <summary>
    /// Get application log entries with optional filtering
    /// </summary>
    /// <param name="entity_id">Filter by entity ID</param>
    /// <param name="before">Get entries before this ID (for pagination)</param>
    /// <param name="after">Get entries after this ID (for real-time updates)</param>
    /// <param name="take">Maximum number of entries to return</param>
    /// <returns>List of log entries</returns>
    [HttpGet("Log")]
    public async Task<IActionResult> GetLog(
        [FromQuery] string? entity_id = null,
        [FromQuery] int? before = null,
        [FromQuery] int? after = null,
        [FromQuery] int? take = null)
    {
        try
        {
            _logger.LogInformation("Retrieving log entries with filters - EntityId: {EntityId}, Before: {Before}, After: {After}, Take: {Take}", 
                entity_id, before, after, take);

            var parameters = new LogQueryParameters
            {
                EntityId = entity_id,
                Before = before,
                After = after,
                Take = take
            };

            var logEntries = await _logLogic.GetLogEntriesAsync(parameters).ConfigureAwait(false);
            return Ok(logEntries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve log entries");
            return StatusCode(500, "An error occurred while retrieving log entries");
        }
    }

    /// <summary>
    /// Add a new log entry
    /// </summary>
    /// <param name="logEntry">Log entry to add</param>
    /// <returns>Success result</returns>
    [HttpPost("Log")]
    public async Task<IActionResult> AddLogEntry([FromBody] LogEntry logEntry)
    {
        try
        {
            _logger.LogInformation("Adding new log entry for entity: {EntityId}, type: {TypeId}", 
                logEntry.EntityId, logEntry.TypeId);

            await _logLogic.AddLogEntryAsync(logEntry).ConfigureAwait(false);
            return Ok(new { message = "Log entry added successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add log entry");
            return StatusCode(500, "An error occurred while adding the log entry");
        }
    }
}
