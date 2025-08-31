namespace WebApi.Controllers;

public class UpdateLegalMatterRequest
{
    public string MatterName { get; set; } = string.Empty;
    public string? ContractType { get; set; }
    public List<string>? Parties { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? GoverningLaw { get; set; }
    public decimal? ContractValue { get; set; }
    public string? Status { get; set; }
    public string? Description { get; set; }
    public Guid? LawyerId { get; set; }
}
