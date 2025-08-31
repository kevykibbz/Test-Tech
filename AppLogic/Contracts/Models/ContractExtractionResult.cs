namespace AppLogic.Contracts.Models;

/// <summary>
/// Represents the result of contract extraction operations
/// </summary>
public class ContractExtractionResult
{
    /// <summary>
    /// The parties involved in the contract
    /// </summary>
    public List<string> Parties { get; set; } = new();

    /// <summary>
    /// Key dates mentioned in the contract
    /// </summary>
    public List<ContractDate> KeyDates { get; set; } = new();

    /// <summary>
    /// Financial terms and amounts
    /// </summary>
    public List<FinancialTerm> FinancialTerms { get; set; } = new();

    /// <summary>
    /// Key obligations and responsibilities
    /// </summary>
    public List<string> KeyObligations { get; set; } = new();

    /// <summary>
    /// Termination clauses and conditions
    /// </summary>
    public List<string> TerminationClauses { get; set; } = new();

    /// <summary>
    /// Intellectual property clauses
    /// </summary>
    public List<string> IntellectualPropertyClauses { get; set; } = new();

    /// <summary>
    /// Confidentiality and non-disclosure terms
    /// </summary>
    public List<string> ConfidentialityTerms { get; set; } = new();

    /// <summary>
    /// Governing law and jurisdiction
    /// </summary>
    public string? GoverningLaw { get; set; }

    /// <summary>
    /// Contract type or category
    /// </summary>
    public string? ContractType { get; set; }

    /// <summary>
    /// Overall contract summary
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Raw extracted text that was analyzed
    /// </summary>
    public string? RawText { get; set; }

    /// <summary>
    /// Timestamp when the extraction was performed
    /// </summary>
    public DateTime ExtractedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents a date mentioned in the contract
/// </summary>
public class ContractDate
{
    /// <summary>
    /// The actual date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Description of what this date represents
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The original text from which this date was extracted
    /// </summary>
    public string OriginalText { get; set; } = string.Empty;
}

/// <summary>
/// Represents a financial term in the contract
/// </summary>
public class FinancialTerm
{
    /// <summary>
    /// The monetary amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The currency code (e.g., USD, EUR)
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Description of what this amount represents
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The original text from which this amount was extracted
    /// </summary>
    public string OriginalText { get; set; } = string.Empty;

    /// <summary>
    /// Whether this is a recurring payment
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Payment frequency if recurring (e.g., monthly, yearly)
    /// </summary>
    public string? PaymentFrequency { get; set; }
}
