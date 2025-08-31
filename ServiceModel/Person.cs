namespace ServiceModel;

public class Person
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string FullName { get; set; }
    public required string Initials { get; set; }
    public bool HasPicture { get; set; }
    public string? PictureUrl { get; set; }
}
