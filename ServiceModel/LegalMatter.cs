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
    DateTime? LastModified = null)
{
    public Guid? LawyerId { get; init; }
    
    public LegalMatter() : this(Guid.NewGuid(), string.Empty)
    {
        CreatedAt = DateTime.UtcNow;
    }

    // Helper properties for easier access
    public bool HasParties => Parties?.Count > 0;
    public bool HasFinancialTerms => ContractValue.HasValue && ContractValue > 0;
    public bool IsActive => Status?.Equals("Active", StringComparison.OrdinalIgnoreCase) == true;
    public bool IsExpired => ExpirationDate.HasValue && ExpirationDate < DateTime.UtcNow;
};
