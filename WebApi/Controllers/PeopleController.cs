using AppLogic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class PeopleController(IPeopleLogic peopleLogic, ILogger<PeopleController> logger) : ControllerBase
{
    private readonly IPeopleLogic _peopleLogic = peopleLogic;
    private readonly ILogger<PeopleController> _logger = logger;

    /// <summary>
    /// Get all people
    /// </summary>
    /// <returns>List of people</returns>
    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        try
        {
            _logger.LogInformation("Retrieving all people");
            var people = await _peopleLogic.GetPeopleAsync().ConfigureAwait(false);
            return Ok(people);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve people");
            return StatusCode(500, "An error occurred while retrieving people");
        }
    }

    /// <summary>
    /// Get a specific person by ID
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>Person details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(string id)
    {
        try
        {
            _logger.LogInformation("Retrieving person with ID: {Id}", id);
            var person = await _peopleLogic.GetPersonByIdAsync(id).ConfigureAwait(false);
            
            if (person == null)
            {
                return NotFound($"Person with ID '{id}' not found");
            }

            return Ok(person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve person with ID: {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the person");
        }
    }
}
