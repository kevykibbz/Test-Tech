namespace DataAccess.DataModel;

public record DbLegalMatter
(
    Guid Id,
    string MatterName,
    string? ContractType = null,
    string? Parties = null, // JSON string representation of List<string>
    DateTime? EffectiveDate = null,
    DateTime? ExpirationDate = null,
    string? GoverningLaw = null,
    decimal? ContractValue = null,
    string? Status = null,
    string? Description = null,
    DateTime CreatedAt = default,
    DateTime? LastModified = null
)
{
    public Guid? LawyerId { get; set; }
};

