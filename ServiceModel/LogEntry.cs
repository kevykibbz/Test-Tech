namespace ServiceModel;

public class LogEntry
{
    public int? Id { get; set; }
    public string? Details { get; set; }
    public string? EntityId { get; set; }
    public string? TypeId { get; set; }
    public DateTime? CreatedAt { get; set; }
}
