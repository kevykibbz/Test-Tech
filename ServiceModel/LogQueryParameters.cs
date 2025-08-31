namespace ServiceModel;

public class LogQueryParameters
{
    public string? EntityId { get; set; }
    public int? Before { get; set; }
    public int? After { get; set; }
    public int? Take { get; set; }
}
