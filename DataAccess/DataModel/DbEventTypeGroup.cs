namespace DataAccess.DataModel;

public class DbEventTypeGroup
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    // Navigation properties
    public ICollection<DbEventType> EventTypes { get; set; } = new List<DbEventType>();
}
