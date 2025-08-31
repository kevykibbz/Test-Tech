using AppLogic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class LegalMatterCategoriesController(ILegalMatterCategoryLogic categoryLogic, ILogger<LegalMatterCategoriesController> logger) : ControllerBase
{
    private readonly ILegalMatterCategoryLogic _categoryLogic = categoryLogic;
    private readonly ILogger<LegalMatterCategoriesController> _logger = logger;

    /// <summary>
    /// Get all legal matter categories
    /// </summary>
    /// <returns>List of legal matter categories</returns>
    [HttpGet]
    public async Task<IActionResult> GetLegalMatterCategories()
    {
        try
        {
            _logger.LogInformation("Retrieving all legal matter categories");
            var categories = await _categoryLogic.GetLegalMatterCategoriesAsync().ConfigureAwait(false);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve legal matter categories");
            return StatusCode(500, "An error occurred while retrieving legal matter categories");
        }
    }

    /// <summary>
    /// Get a specific legal matter category by ID
    /// </summary>
    /// <param name="id">Legal matter category ID</param>
    /// <returns>Legal matter category details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLegalMatterCategory(string id)
    {
        try
        {
            _logger.LogInformation("Retrieving legal matter category with ID: {Id}", id);
            var category = await _categoryLogic.GetLegalMatterCategoryByIdAsync(id).ConfigureAwait(false);
            
            if (category == null)
            {
                return NotFound($"Legal matter category with ID '{id}' not found");
            }

            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve legal matter category with ID: {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the legal matter category");
        }
    }
}
