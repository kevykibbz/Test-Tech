using AppLogic;
using Microsoft.AspNetCore.Mvc;
using ServiceModel;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LegalMatterController : ControllerBase
{
    private readonly ILegalLogic _legalLogic;
    private readonly ILogger<LegalMatterController> _logger;

    public LegalMatterController(ILegalLogic legalLogic, ILogger<LegalMatterController> logger)
    {
        _legalLogic = legalLogic;
        _logger = logger;
    }

    /// <summary>
    /// Get all legal matters with pagination
    /// </summary>
    /// <param name="skip">Number of records to skip</param>
    /// <param name="take">Number of records to take</param>
    /// <returns>List of legal matters</returns>
    [HttpGet]
    public async Task<IActionResult> Get(int skip = 0, int take = 100)
    {
        try
        {
            _logger.LogInformation("Retrieving legal matters with skip: {Skip}, take: {Take}", skip, take);
            var result = await _legalLogic.GetMattersAsync(skip, take).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve legal matters");
            return StatusCode(500, new { error = "Failed to retrieve legal matters", details = ex.Message });
        }
    }

    /// <summary>
    /// Get a specific legal matter by ID
    /// </summary>
    /// <param name="id">The legal matter ID</param>
    /// <returns>Legal matter details</returns>
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            _logger.LogInformation("Retrieving legal matter with ID: {Id}", id);
            var result = await _legalLogic.GetMatterAsync(id).ConfigureAwait(false);
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve legal matter with ID: {Id}", id);
            return StatusCode(500, new { error = "Failed to retrieve legal matter", details = ex.Message });
        }
    }

    /// <summary>
    /// Create a new legal matter
    /// </summary>
    /// <param name="matter">The legal matter to create</param>
    /// <returns>Created legal matter</returns>
    [HttpPost]
    public async Task<IActionResult> Create(LegalMatter matter)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(matter.MatterName))
            {
                return BadRequest(new { error = "Matter name is required" });
            }

            _logger.LogInformation("Creating new legal matter: {MatterName}", matter.MatterName);
            
            // Ensure we have a proper creation timestamp
            var matterToCreate = matter with { CreatedAt = DateTime.UtcNow };
            
            await _legalLogic.CreateMatterAsync(matterToCreate).ConfigureAwait(false);
            return CreatedAtAction(nameof(Get), new { id = matterToCreate.Id }, matterToCreate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create legal matter: {MatterName}", matter.MatterName);
            return StatusCode(500, new { error = "Failed to create legal matter", details = ex.Message });
        }
    }

    /// <summary>
    /// Get a sample legal matter object for testing
    /// </summary>
    /// <returns>Sample legal matter</returns>
    [HttpGet("sample")]
    public IActionResult GetSample()
    {
        try
        {
            _logger.LogInformation("Generating sample legal matter");
            
            var sampleMatter = new LegalMatter(
                Id: Guid.NewGuid(),
                MatterName: "Allied Esports Entertainment License Agreement",
                ContractType: "Content License Agreement",
                Parties: new List<string> 
                { 
                    "Allied Esports Entertainment, Inc.", 
                    "Content Licensing Partner LLC" 
                },
                EffectiveDate: new DateTime(2019, 8, 15),
                ExpirationDate: new DateTime(2024, 8, 14),
                GoverningLaw: "Delaware",
                ContractValue: 125000.00m,
                Status: "Active",
                Description: "Content licensing agreement for entertainment and broadcasting rights",
                CreatedAt: DateTime.UtcNow
            );

            return Ok(sampleMatter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate sample legal matter");
            return StatusCode(500, new { error = "Failed to generate sample legal matter", details = ex.Message });
        }
    }

    /// <summary>
    /// Get total count of legal matters
    /// </summary>
    /// <returns>Total count</returns>
    [HttpGet("Total")]
    public async Task<IActionResult> GetTotal()
    {
        try
        {
            _logger.LogInformation("Getting total legal matter count");
            var count = await _legalLogic.GetTotalCountAsync().ConfigureAwait(false);
            return Ok(new { total = count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get total legal matter count");
            return StatusCode(500, new { error = "Failed to get total count", details = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing legal matter
    /// </summary>
    /// <param name="id">The legal matter ID</param>
    /// <param name="request">The updated legal matter data</param>
    /// <returns>Updated legal matter</returns>
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateLegalMatterRequest request)
    {
        try
        {
            _logger.LogInformation("Received update request for legal matter {Id}", id);
            _logger.LogInformation("Request body: {@Request}", request);
            
            if (request is null)
            {
                _logger.LogWarning("Request object is null");
                return BadRequest(new { error = "Request data is required" });
            }
            
            if (string.IsNullOrWhiteSpace(request.MatterName))
            {
                _logger.LogWarning("Matter name is missing or empty");
                return BadRequest(new { error = "Matter name is required" });
            }

            _logger.LogInformation("Updating legal matter {Id}: {MatterName}", id, request.MatterName);
            
            // Create a new matter object with the correct ID and preserve creation date
            try
            {
                var existingMatter = await _legalLogic.GetMatterAsync(id).ConfigureAwait(false);
                _logger.LogInformation("Found existing matter: {@ExistingMatter}", existingMatter);
                
                // Create updated matter from request data
                var updatedMatter = new LegalMatter(
                    Id: id,
                    MatterName: request.MatterName,
                    ContractType: request.ContractType,
                    Parties: request.Parties,
                    EffectiveDate: request.EffectiveDate,
                    ExpirationDate: request.ExpirationDate,
                    GoverningLaw: request.GoverningLaw,
                    ContractValue: request.ContractValue,
                    Status: request.Status,
                    Description: request.Description,
                    CreatedAt: existingMatter.CreatedAt, // Always use the existing creation date
                    LastModified: DateTime.UtcNow
                )
                {
                    LawyerId = request.LawyerId
                };
                
                _logger.LogInformation("Prepared matter for update: {@UpdatedMatter}", updatedMatter);
                
                var result = await _legalLogic.UpdateMatterAsync(updatedMatter).ConfigureAwait(false);
                _logger.LogInformation("Update result: {@Result}", result);
                
                return result is null ? NotFound() : Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Legal matter {Id} not found", id);
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update legal matter {Id}", id);
            return StatusCode(500, new { error = "Failed to update legal matter", details = ex.Message });
        }
    }

    /// <summary>
    /// Delete a legal matter
    /// </summary>
    /// <param name="id">The legal matter ID</param>
    /// <returns>Success result</returns>
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting legal matter {Id}", id);
            
            var success = await _legalLogic.DeleteMatterAsync(id).ConfigureAwait(false);
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete legal matter {Id}", id);
            return StatusCode(500, new { error = "Failed to delete legal matter", details = ex.Message });
        }
    }

    /// <summary>
    /// Assign a lawyer to a legal matter
    /// </summary>
    /// <param name="id">The legal matter ID</param>
    /// <param name="request">The lawyer assignment request</param>
    /// <returns>Updated legal matter</returns>
    [HttpPut("{id:Guid}/assign-lawyer")]
    public async Task<IActionResult> AssignLawyer(Guid id, [FromBody] AssignLawyerRequest request)
    {
        if (request is null)
        {
            return BadRequest(new { error = "Request body is required" });
        }

        try
        {
            _logger.LogInformation("Assigning lawyer {LawyerId} to legal matter {Id}", request.LawyerId, id);
            
            var matter = await _legalLogic.GetMatterAsync(id).ConfigureAwait(false);
            if (matter is null)
            {
                return NotFound(new { error = "Legal matter not found" });
            }

            var updatedMatter = matter with { LawyerId = request.LawyerId };
            var result = await _legalLogic.UpdateMatterAsync(updatedMatter).ConfigureAwait(false);
            
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign lawyer {LawyerId} to legal matter {Id}", request.LawyerId, id);
            return StatusCode(500, new { error = "Failed to assign lawyer", details = ex.Message });
        }
    }

    /// <summary>
    /// Unassign lawyer from a legal matter
    /// </summary>
    /// <param name="id">The legal matter ID</param>
    /// <returns>Updated legal matter</returns>
    [HttpPut("{id:Guid}/unassign-lawyer")]
    public async Task<IActionResult> UnassignLawyer(Guid id)
    {
        try
        {
            _logger.LogInformation("Unassigning lawyer from legal matter {Id}", id);
            
            var matter = await _legalLogic.GetMatterAsync(id).ConfigureAwait(false);
            if (matter is null)
            {
                return NotFound(new { error = "Legal matter not found" });
            }

            var updatedMatter = matter with { LawyerId = null };
            var result = await _legalLogic.UpdateMatterAsync(updatedMatter).ConfigureAwait(false);
            
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to unassign lawyer from legal matter {Id}", id);
            return StatusCode(500, new { error = "Failed to unassign lawyer", details = ex.Message });
        }
    }

    /// <summary>
    /// Get legal matters assigned to a specific lawyer
    /// </summary>
    /// <param name="lawyerId">The lawyer ID</param>
    /// <returns>List of legal matters assigned to the lawyer</returns>
    [HttpGet("by-lawyer/{lawyerId:Guid}")]
    public async Task<IActionResult> GetByLawyer(Guid lawyerId)
    {
        try
        {
            _logger.LogInformation("Retrieving legal matters for lawyer {LawyerId}", lawyerId);
            var result = await _legalLogic.GetMattersByLawyerAsync(lawyerId).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve legal matters for lawyer {LawyerId}", lawyerId);
            return StatusCode(500, new { error = "Failed to retrieve legal matters for lawyer", details = ex.Message });
        }
    }
}

/// <summary>
/// Request object for assigning a lawyer to a legal matter
/// </summary>
public class AssignLawyerRequest
{
    public Guid LawyerId { get; set; }
}
