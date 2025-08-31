namespace DataAccess.DataModel;

public class DbEventType
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string GroupId { get; set; }
    
    // Navigation property
    public DbEventTypeGroup? Group { get; set; }
}
