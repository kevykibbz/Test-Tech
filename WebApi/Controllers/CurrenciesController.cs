using AppLogic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class CurrenciesController(ICurrencyLogic currencyLogic, ILogger<CurrenciesController> logger) : ControllerBase
{
    private readonly ICurrencyLogic _currencyLogic = currencyLogic;
    private readonly ILogger<CurrenciesController> _logger = logger;

    /// <summary>
    /// Get all available currencies
    /// </summary>
    /// <returns>List of currencies</returns>
    [HttpGet]
    public async Task<IActionResult> GetCurrencies()
    {
        try
        {
            _logger.LogInformation("Retrieving all currencies");
            var currencies = await _currencyLogic.GetCurrenciesAsync().ConfigureAwait(false);
            return Ok(currencies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve currencies");
            return StatusCode(500, "An error occurred while retrieving currencies");
        }
    }

    /// <summary>
    /// Get a specific currency by ID
    /// </summary>
    /// <param name="id">Currency ID (e.g., USD, EUR, GBP)</param>
    /// <returns>Currency details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCurrency(string id)
    {
        try
        {
            _logger.LogInformation("Retrieving currency with ID: {Id}", id);
            var currency = await _currencyLogic.GetCurrencyByIdAsync(id).ConfigureAwait(false);
            
            if (currency == null)
            {
                return NotFound($"Currency with ID '{id}' not found");
            }

            return Ok(currency);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve currency with ID: {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the currency");
        }
    }
}
