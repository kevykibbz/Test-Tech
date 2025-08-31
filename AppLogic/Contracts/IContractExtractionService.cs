using AppLogic.Contracts.Models;

namespace AppLogic.Contracts;

/// <summary>
/// Service for extracting structured information from contracts
/// </summary>
public interface IContractExtractionService
{
    /// <summary>
    /// Extracts comprehensive information from a contract
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Structured contract information</returns>
    Task<ContractExtractionResult> ExtractContractInformationAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts comprehensive information from provided contract text
    /// </summary>
    /// <param name="contractText">The contract text to analyze</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Structured contract information</returns>
    Task<ContractExtractionResult> ExtractContractInformationFromTextAsync(string contractText, CancellationToken cancellationToken = default);
}
