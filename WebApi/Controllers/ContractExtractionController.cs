using AppLogic.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// Controller for contract extraction operations
/// </summary>
[ApiController]
[Route("[controller]")]
public class ContractExtractionController(
    IContractExtractionService contractExtractionService,
    ILogger<ContractExtractionController> logger) : ControllerBase
{
    private readonly IContractExtractionService _contractExtractionService = contractExtractionService;
    private readonly ILogger<ContractExtractionController> _logger = logger;

    /// <summary>
    /// Extracts comprehensive information from the default contract file
    /// </summary>
    /// <returns>Complete contract extraction result</returns>
    [HttpGet("")]
    public async Task<IActionResult> ExtractContractInformation(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting contract extraction via API for default contract");

            var result = await _contractExtractionService.ExtractContractInformationAsync(cancellationToken);

            _logger.LogInformation("Contract extraction completed successfully via API");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract contract information via API");
            return StatusCode(500, new { error = "Failed to extract contract information", details = ex.Message });
        }
    }

    /// <summary>
    /// Extracts comprehensive information from uploaded contract text
    /// </summary>
    /// <param name="contractText">The contract text to analyze</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Complete contract extraction result</returns>
    [HttpPost("extract-text")]
    public async Task<IActionResult> ExtractFromText([FromBody] string contractText, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(contractText))
            {
                return BadRequest(new { error = "Contract text cannot be empty" });
            }

            _logger.LogInformation("Starting contract extraction via API for provided text (length: {Length})", contractText.Length);

            var result = await _contractExtractionService.ExtractContractInformationFromTextAsync(contractText, cancellationToken);

            _logger.LogInformation("Contract extraction completed successfully via API for provided text");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract contract information from provided text via API");
            return StatusCode(500, new { error = "Failed to extract contract information", details = ex.Message });
        }
    }

    /// <summary>
    /// Extracts comprehensive information from an uploaded contract file
    /// </summary>
    /// <param name="file">The contract file to analyze (PDF or text)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Complete contract extraction result</returns>
    [HttpPost("extract-file")]
    public async Task<IActionResult> ExtractFromFile(IFormFile file, CancellationToken cancellationToken = default)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "No file provided or file is empty" });
            }

            var allowedExtensions = new[] { ".txt", ".pdf" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { error = "Only PDF and TXT files are supported" });
            }

            _logger.LogInformation("Starting contract extraction via API for uploaded file: {FileName} (size: {Size} bytes)", 
                file.FileName, file.Length);

            string contractText;

            if (fileExtension == ".txt")
            {
                using var reader = new StreamReader(file.OpenReadStream());
                contractText = await reader.ReadToEndAsync(cancellationToken);
            }
            else
            {
                // For PDF files, we would need a PDF reader library
                // For now, return an informative message
                return BadRequest(new { error = "PDF file processing is not yet implemented. Please use the default contract extraction endpoint or provide text directly." });
            }

            if (string.IsNullOrWhiteSpace(contractText))
            {
                return BadRequest(new { error = "No readable text found in the uploaded file" });
            }

            var result = await _contractExtractionService.ExtractContractInformationFromTextAsync(contractText, cancellationToken);

            _logger.LogInformation("Contract extraction completed successfully via API for uploaded file: {FileName}", file.FileName);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract contract information from uploaded file via API");
            return StatusCode(500, new { error = "Failed to extract contract information", details = ex.Message });
        }
    }
}
