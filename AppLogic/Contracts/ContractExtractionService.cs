using AppLogic.Contracts.Models;
using AppLogic.Ollama.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppLogic.Contracts;

/// <summary>
/// Service for extracting structured information from contracts
/// </summary>
public class ContractExtractionService(
    IContractService contractService,
    IOllamaClient ollamaClient,
    ILogger<ContractExtractionService> logger) : IContractExtractionService
{
    private readonly IContractService _contractService = contractService;
    private readonly IOllamaClient _ollamaClient = ollamaClient;
    private readonly ILogger<ContractExtractionService> _logger = logger;

    public async Task<ContractExtractionResult> ExtractContractInformationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting comprehensive contract extraction");

            var pagedContractText = await _contractService.RetrieveContractTextAsync();

            if (pagedContractText == null || !pagedContractText.HasText)
            {
                throw new InvalidOperationException("Contract text is empty or null");
            }

            return await ExtractContractInformationFromTextAsync(pagedContractText, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract contract information");
            throw new InvalidOperationException("Failed to extract contract information", ex);
        }
    }

    public async Task<ContractExtractionResult> ExtractContractInformationFromTextAsync(string contractText, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(contractText))
        {
            throw new ArgumentException("Contract text cannot be null or empty", nameof(contractText));
        }

        try
        {
            _logger.LogInformation("Starting contract extraction from provided text (length: {Length})", contractText.Length);

            var contractDocument = new ContractDocument
            {
                TotalPages = 1,
                Pages = new List<ContractPage>
                {
                    new ContractPage
                    {
                        PageNumber = 1,
                        Text = contractText
                    }
                }
            };

            return await ExtractContractInformationFromTextAsync(contractDocument, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract contract information from provided text");
            throw new InvalidOperationException("Failed to extract contract information from provided text", ex);
        }
    }

    private async Task<ContractExtractionResult> ExtractContractInformationFromTextAsync(ContractDocument contractDocument, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = new ContractExtractionResult
            {
                RawText = contractDocument.FullText,
                ExtractedAt = DateTime.UtcNow
            };

            // Extract parties involved
            result.Parties = await ExtractPartiesAsync(contractDocument.FullText, cancellationToken);

            // Extract key dates
            result.KeyDates = await ExtractKeyDatesAsync(contractDocument.FullText, cancellationToken);

            // Extract financial terms
            result.FinancialTerms = await ExtractFinancialTermsAsync(contractDocument.FullText, cancellationToken);

            // Extract key obligations
            result.KeyObligations = await ExtractKeyObligationsAsync(contractDocument.FullText, cancellationToken);

            // Extract termination clauses
            result.TerminationClauses = await ExtractTerminationClausesAsync(contractDocument.FullText, cancellationToken);

            // Extract intellectual property clauses
            result.IntellectualPropertyClauses = await ExtractIntellectualPropertyClausesAsync(contractDocument.FullText, cancellationToken);

            // Extract confidentiality terms
            result.ConfidentialityTerms = await ExtractConfidentialityTermsAsync(contractDocument.FullText, cancellationToken);

            // Extract governing law
            result.GoverningLaw = await ExtractGoverningLawAsync(contractDocument.FullText, cancellationToken);

            // Extract contract type
            result.ContractType = await ExtractContractTypeAsync(contractDocument.FullText, cancellationToken);

            // Generate summary
            result.Summary = await GenerateContractSummaryAsync(contractDocument.FullText, cancellationToken);

            _logger.LogInformation("Successfully extracted contract information from {Pages} pages with {Characters} characters",
                contractDocument.TotalPages, contractDocument.TotalCharacterCount);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract contract information from text");
            throw new InvalidOperationException("Failed to extract contract information from text", ex);
        }
    }

    private async Task<List<string>> ExtractPartiesAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Extract all parties involved in this contract.

Rules:
- Return ONLY the party names, one per line
- Include full legal entity names (e.g., ""Company Name, Inc."", ""John Doe"", ""ABC Corp."")
- Do not include any explanations or additional text
- If no clear parties are found, return ""Unknown Party""

Contract text:
{contractText}

Parties:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            return ParseResponseToList(response);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extract parties, returning empty list");
            return new List<string>();
        }
    }

    private async Task<List<ContractDate>> ExtractKeyDatesAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Extract all important dates from this contract.

Rules:
- Return dates in the format: YYYY-MM-DD|Description|Original Text
- Include effective dates, expiration dates, deadlines, renewal dates, etc.
- One date per line
- If no dates are found, return ""None""

Contract text:
{contractText}

Key Dates:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            return ParseResponseToContractDates(response);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extract key dates, returning empty list");
            return new List<ContractDate>();
        }
    }

    private async Task<List<FinancialTerm>> ExtractFinancialTermsAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Extract all financial terms and amounts from this contract.

Rules:
- Return in format: Amount|Currency|Description|Original Text|IsRecurring|PaymentFrequency
- Use USD as default currency if not specified
- IsRecurring should be true/false
- PaymentFrequency should be ""monthly"", ""yearly"", ""one-time"", etc.
- One financial term per line
- If no financial terms found, return ""None""

Contract text:
{contractText}

Financial Terms:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            return ParseResponseToFinancialTerms(response);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extract financial terms, returning empty list");
            return new List<FinancialTerm>();
        }
    }

    private async Task<List<string>> ExtractKeyObligationsAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Extract the key obligations and responsibilities from this contract.

Rules:
- Return ONLY the obligation descriptions, one per line
- Focus on main duties, deliverables, and responsibilities
- Keep descriptions concise but complete
- Do not include any explanations
- If no clear obligations found, return ""No specific obligations identified""

Contract text:
{contractText}

Key Obligations:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            return ParseResponseToList(response);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extract key obligations, returning empty list");
            return new List<string>();
        }
    }

    private async Task<List<string>> ExtractTerminationClausesAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Extract all termination clauses and conditions from this contract.

Rules:
- Return ONLY the termination conditions, one per line
- Include notice periods, breach conditions, and termination procedures
- Keep descriptions clear and concise
- Do not include any explanations
- If no termination clauses found, return ""No termination clauses specified""

Contract text:
{contractText}

Termination Clauses:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            return ParseResponseToList(response);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extract termination clauses, returning empty list");
            return new List<string>();
        }
    }

    private async Task<List<string>> ExtractIntellectualPropertyClausesAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Extract all intellectual property clauses from this contract.

Rules:
- Return ONLY IP-related clauses, one per line
- Include copyright, trademark, patent, trade secret provisions
- Focus on ownership, licensing, and usage rights
- Do not include any explanations
- If no IP clauses found, return ""No intellectual property clauses specified""

Contract text:
{contractText}

Intellectual Property Clauses:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            return ParseResponseToList(response);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extract IP clauses, returning empty list");
            return new List<string>();
        }
    }

    private async Task<List<string>> ExtractConfidentialityTermsAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Extract all confidentiality and non-disclosure terms from this contract.

Rules:
- Return ONLY confidentiality provisions, one per line
- Include NDAs, confidentiality obligations, and disclosure restrictions
- Keep descriptions clear and concise
- Do not include any explanations
- If no confidentiality terms found, return ""No confidentiality terms specified""

Contract text:
{contractText}

Confidentiality Terms:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            return ParseResponseToList(response);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extract confidentiality terms, returning empty list");
            return new List<string>();
        }
    }

    private async Task<string?> ExtractGoverningLawAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Extract the governing law and jurisdiction from this contract.

Rules:
- Return ONLY the jurisdiction/governing law (e.g., ""New York"", ""Delaware"", ""California"", ""England and Wales"")
- Do not include any explanations
- If no governing law specified, return ""Not specified""

Contract text:
{contractText}

Governing Law:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            var result = response?.Trim();
            return string.IsNullOrEmpty(result) ? "Not specified" : result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extract governing law");
            return "Not specified";
        }
    }

    private async Task<string?> ExtractContractTypeAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Identify the type of this contract.

Rules:
- Return ONLY the contract type (e.g., ""Service Agreement"", ""License Agreement"", ""Employment Contract"", ""NDA"", ""Purchase Agreement"")
- Do not include any explanations
- If type cannot be determined, return ""General Contract""

Contract text:
{contractText}

Contract Type:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            var result = response?.Trim();
            return string.IsNullOrEmpty(result) ? "General Contract" : result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extract contract type");
            return "General Contract";
        }
    }

    private async Task<string?> GenerateContractSummaryAsync(string contractText, CancellationToken cancellationToken)
    {
        var prompt = $@"
You are a legal document analysis expert. Generate a concise summary of this contract.

Rules:
- Provide a 2-3 sentence summary covering the main purpose and key terms
- Focus on what the contract is about and the primary obligations
- Use clear, professional language
- Do not exceed 200 words

Contract text:
{contractText}

Summary:";

        try
        {
            var response = await _ollamaClient.GenerateAsync(prompt, cancellationToken);
            return response?.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to generate contract summary");
            return "Summary not available";
        }
    }

    private static List<string> ParseResponseToList(string response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return new List<string>();

        return response
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line) && 
                          !line.Equals("None", StringComparison.OrdinalIgnoreCase) &&
                          !line.Equals("No specific obligations identified", StringComparison.OrdinalIgnoreCase) &&
                          !line.Equals("No termination clauses specified", StringComparison.OrdinalIgnoreCase) &&
                          !line.Equals("No intellectual property clauses specified", StringComparison.OrdinalIgnoreCase) &&
                          !line.Equals("No confidentiality terms specified", StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private static List<ContractDate> ParseResponseToContractDates(string response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return new List<ContractDate>();

        var dates = new List<ContractDate>();
        var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine) || 
                trimmedLine.Equals("None", StringComparison.OrdinalIgnoreCase))
                continue;

            var parts = trimmedLine.Split('|');
            if (parts.Length >= 3)
            {
                if (DateTime.TryParse(parts[0].Trim(), out var parsedDate))
                {
                    dates.Add(new ContractDate
                    {
                        Date = parsedDate,
                        Description = parts[1].Trim(),
                        OriginalText = parts.Length > 2 ? parts[2].Trim() : ""
                    });
                }
            }
        }

        return dates;
    }

    private static List<FinancialTerm> ParseResponseToFinancialTerms(string response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return new List<FinancialTerm>();

        var terms = new List<FinancialTerm>();
        var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine) || 
                trimmedLine.Equals("None", StringComparison.OrdinalIgnoreCase))
                continue;

            var parts = trimmedLine.Split('|');
            if (parts.Length >= 4)
            {
                if (decimal.TryParse(parts[0].Trim().Replace("$", "").Replace(",", ""), out var amount))
                {
                    var isRecurring = parts.Length > 4 && bool.TryParse(parts[4].Trim(), out var recurring) && recurring;
                    
                    terms.Add(new FinancialTerm
                    {
                        Amount = amount,
                        Currency = parts[1].Trim(),
                        Description = parts[2].Trim(),
                        OriginalText = parts[3].Trim(),
                        IsRecurring = isRecurring,
                        PaymentFrequency = parts.Length > 5 ? parts[5].Trim() : null
                    });
                }
            }
        }

        return terms;
    }
}
