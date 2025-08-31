namespace ServiceModel;

public record Lawyer(
    Guid Id,
    string FirstName,
    string LastName,
    string CompanyName,
    DateTime CreatedAt = default,
    DateTime? LastModified = null)
{
    public Lawyer() : this(Guid.NewGuid(), string.Empty, string.Empty, string.Empty)
    {
        CreatedAt = DateTime.UtcNow;
    }

    // Helper properties
    public string FullName => $"{FirstName} {LastName}".Trim();
    public bool IsValid => !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
};
