namespace ServiceModel;

public record LegalMatter(
    Guid Id, 
    string MatterName, 
    string? ContractType = null,
    List<string>? Parties = null,
    DateTime? EffectiveDate = null,
    DateTime? ExpirationDate = null,
    string? GoverningLaw = null,
    decimal? ContractValue = null,
    string? Status = null,
    string? Description = null,
    DateTime CreatedAt = default,
    DateTime? LastModified = null,
    Guid? CategoryId = null,
    Guid? ManagerId = null,
    DateTime? DueDate = null,
    decimal? EstimatedCost = null,
    string? Currency = null)
{
    public Guid? LawyerId { get; init; }
    
    // Lawyer enrichment properties
    public string? LawyerFirstName { get; init; }
    public string? LawyerLastName { get; init; }
    public string? LawyerCompany { get; init; }
    
    public LegalMatter() : this(Guid.NewGuid(), string.Empty)
    {
        CreatedAt = DateTime.UtcNow;
    }

    // Helper properties for easier access
    public string? LawyerFullName => !string.IsNullOrWhiteSpace(LawyerFirstName) && !string.IsNullOrWhiteSpace(LawyerLastName) 
        ? $"{LawyerFirstName} {LawyerLastName}".Trim() 
        : null;
    public bool HasParties => Parties?.Count > 0;
    public bool HasFinancialTerms => ContractValue.HasValue && ContractValue > 0;
    public bool IsActive => Status?.Equals("Active", StringComparison.OrdinalIgnoreCase) == true;
    public bool IsExpired => ExpirationDate.HasValue && ExpirationDate < DateTime.UtcNow;
    public bool HasDueDate => DueDate.HasValue;
    public bool IsOverdue => DueDate.HasValue && DueDate < DateTime.UtcNow;
    public bool HasEstimatedCost => EstimatedCost.HasValue && EstimatedCost > 0;
};
