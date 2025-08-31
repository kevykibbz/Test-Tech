namespace AppLogic.Contracts.Models;

/// <summary>
/// Represents contract text extracted from a PDF document, organized by pages
/// </summary>
public class ContractDocument
{
    /// <summary>
    /// The total number of pages in the contract
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// The individual pages of the contract with their text content
    /// </summary>
    public List<ContractPage> Pages { get; set; } = [];

    /// <summary>
    /// The complete text content of all pages combined
    /// </summary>
    public string FullText => string.Join(Environment.NewLine + Environment.NewLine, Pages.Select(p => p.Text));

    /// <summary>
    /// The total character count across all pages
    /// </summary>
    public int TotalCharacterCount => Pages.Sum(p => p.Text.Length);

    /// <summary>
    /// Whether the contract has any extractable text
    /// </summary>
    public bool HasText => Pages.Any(p => !string.IsNullOrWhiteSpace(p.Text));
}

/// <summary>
/// Represents a single page of contract text
/// </summary>
public class ContractPage
{
    /// <summary>
    /// The page number (1-based)
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// The extracted text content from this page
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// The character count for this page
    /// </summary>
    public int CharacterCount => Text.Length;

    /// <summary>
    /// Whether this page has any extractable text
    /// </summary>
    public bool HasText => !string.IsNullOrWhiteSpace(Text);
}
