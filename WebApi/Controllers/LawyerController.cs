using AppLogic;
using Microsoft.AspNetCore.Mvc;
using ServiceModel;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class LawyerController : ControllerBase
{
    private readonly ILawyerLogic _lawyerLogic;
    private readonly ILogger<LawyerController> _logger;

    public LawyerController(ILawyerLogic lawyerLogic, ILogger<LawyerController> logger)
    {
        _lawyerLogic = lawyerLogic;
        _logger = logger;
    }

    /// <summary>
    /// Get all lawyers with pagination
    /// </summary>
    /// <param name="skip">Number of records to skip</param>
    /// <param name="take">Number of records to take</param>
    /// <returns>List of lawyers</returns>
    [HttpGet]
    public async Task<IActionResult> Get(int skip = 0, int take = 100)
    {
        try
        {
            _logger.LogInformation("Retrieving lawyers with skip: {Skip}, take: {Take}", skip, take);
            var result = await _lawyerLogic.GetLawyersAsync(skip, take).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve lawyers");
            return StatusCode(500, new { error = "Failed to retrieve lawyers", details = ex.Message });
        }
    }

    /// <summary>
    /// Get a specific lawyer by ID
    /// </summary>
    /// <param name="id">The lawyer ID</param>
    /// <returns>Lawyer details with assigned legal matters</returns>
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            _logger.LogInformation("Retrieving lawyer with ID: {Id}", id);
            var lawyer = await _lawyerLogic.GetLawyerAsync(id).ConfigureAwait(false);
            if (lawyer is null)
            {
                return NotFound();
            }

            var assignedMatters = await _lawyerLogic.GetLawyerMattersAsync(id).ConfigureAwait(false);
            
            var response = new
            {
                lawyer.Id,
                lawyer.FirstName,
                lawyer.LastName,
                lawyer.CompanyName,
                lawyer.CreatedAt,
                lawyer.LastModified,
                AssignedMatters = assignedMatters
            };
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve lawyer with ID: {Id}", id);
            return StatusCode(500, new { error = "Failed to retrieve lawyer", details = ex.Message });
        }
    }

    /// <summary>
    /// Create a new lawyer
    /// </summary>
    /// <param name="lawyer">The lawyer to create</param>
    /// <returns>Created lawyer</returns>
    [HttpPost]
    public async Task<IActionResult> Create(Lawyer lawyer)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(lawyer.FirstName) || string.IsNullOrWhiteSpace(lawyer.LastName))
            {
                return BadRequest(new { error = "First name and last name are required" });
            }

            _logger.LogInformation("Creating new lawyer: {FirstName} {LastName}", lawyer.FirstName, lawyer.LastName);
            
            var result = await _lawyerLogic.CreateLawyerAsync(lawyer).ConfigureAwait(false);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create lawyer: {FirstName} {LastName}", lawyer.FirstName, lawyer.LastName);
            return StatusCode(500, new { error = "Failed to create lawyer", details = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing lawyer
    /// </summary>
    /// <param name="id">The lawyer ID</param>
    /// <param name="lawyer">The updated lawyer data</param>
    /// <returns>Updated lawyer</returns>
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, Lawyer lawyer)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(lawyer);
            
            if (string.IsNullOrWhiteSpace(lawyer.FirstName) || string.IsNullOrWhiteSpace(lawyer.LastName))
            {
                return BadRequest(new { error = "First name and last name are required" });
            }

            _logger.LogInformation("Updating lawyer {Id}: {FirstName} {LastName}", id, lawyer.FirstName, lawyer.LastName);
            
            // Create a new lawyer object with the correct ID
            var updatedLawyer = lawyer with { Id = id };
            
            var result = await _lawyerLogic.UpdateLawyerAsync(updatedLawyer).ConfigureAwait(false);
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update lawyer {Id}: {FirstName} {LastName}", id, lawyer.FirstName, lawyer.LastName);
            return StatusCode(500, new { error = "Failed to update lawyer", details = ex.Message });
        }
    }

    /// <summary>
    /// Delete a lawyer
    /// </summary>
    /// <param name="id">The lawyer ID</param>
    /// <returns>Success result</returns>
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting lawyer {Id}", id);
            
            var success = await _lawyerLogic.DeleteLawyerAsync(id).ConfigureAwait(false);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete lawyer {Id}", id);
            return StatusCode(500, new { error = "Failed to delete lawyer", details = ex.Message });
        }
    }

    /// <summary>
    /// Assign legal matters to a lawyer
    /// </summary>
    /// <param name="lawyerId">The lawyer ID</param>
    /// <param name="matterIds">List of legal matter IDs to assign</param>
    /// <returns>Success result</returns>
    [HttpPost("{lawyerId:Guid}/matters")]
    public async Task<IActionResult> AssignMatters(Guid lawyerId, [FromBody] List<Guid> matterIds)
    {
        try
        {
            if (!matterIds.Any())
            {
                return BadRequest(new { error = "At least one matter ID is required" });
            }

            _logger.LogInformation("Assigning {Count} matters to lawyer {LawyerId}", matterIds.Count, lawyerId);
            
            await _lawyerLogic.AssignMattersToLawyerAsync(lawyerId, matterIds).ConfigureAwait(false);
            return Ok(new { message = $"Successfully assigned {matterIds.Count} matters to lawyer" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign matters to lawyer {LawyerId}", lawyerId);
            return StatusCode(500, new { error = "Failed to assign matters to lawyer", details = ex.Message });
        }
    }

    /// <summary>
    /// Get all matters assigned to a lawyer
    /// </summary>
    /// <param name="lawyerId">The lawyer ID</param>
    /// <returns>List of legal matters assigned to the lawyer</returns>
    [HttpGet("{lawyerId:Guid}/matters")]
    public async Task<IActionResult> GetLawyerMatters(Guid lawyerId)
    {
        try
        {
            _logger.LogInformation("Retrieving matters for lawyer {LawyerId}", lawyerId);
            var result = await _lawyerLogic.GetLawyerMattersAsync(lawyerId).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve matters for lawyer {LawyerId}", lawyerId);
            return StatusCode(500, new { error = "Failed to retrieve lawyer matters", details = ex.Message });
        }
    }
}
