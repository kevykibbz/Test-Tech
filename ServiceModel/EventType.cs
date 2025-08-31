namespace ServiceModel;

public class EventType
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string GroupId { get; set; }
}
