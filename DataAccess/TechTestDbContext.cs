using DataAccess.DataModel;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class TechTestDbContext(DbContextOptions<TechTestDbContext> options) : DbContext(options)
{
    public DbSet<DbLegalMatter> Matter { get; set; } = null!;
    public DbSet<DbLawyer> Lawyer { get; set; } = null!;
    public DbSet<DbCurrency> Currency { get; set; } = null!;
    public DbSet<DbEventType> EventType { get; set; } = null!;
    public DbSet<DbEventTypeGroup> EventTypeGroup { get; set; } = null!;
    public DbSet<DbPerson> Person { get; set; } = null!;
    public DbSet<DbLogEntry> LogEntry { get; set; } = null!;
    public DbSet<DbLegalMatterCategory> LegalMatterCategory { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<DbEventType>()
            .HasOne(e => e.Group)
            .WithMany(g => g.EventTypes)
            .HasForeignKey(e => e.GroupId);

        // Configure LogEntry auto-generated ID
        modelBuilder.Entity<DbLogEntry>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        // Seed some default data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed currencies
        modelBuilder.Entity<DbCurrency>().HasData(
            new DbCurrency { Id = "USD", Symbol = "$", Name = "US Dollar", DecimalDigits = 2 },
            new DbCurrency { Id = "EUR", Symbol = "€", Name = "Euro", DecimalDigits = 2 },
            new DbCurrency { Id = "GBP", Symbol = "£", Name = "British Pound", DecimalDigits = 2 },
            new DbCurrency { Id = "NZD", Symbol = "NZ$", Name = "New Zealand Dollar", DecimalDigits = 2 },
            new DbCurrency { Id = "AUD", Symbol = "A$", Name = "Australian Dollar", DecimalDigits = 2 }
        );

        // Seed event type groups
        modelBuilder.Entity<DbEventTypeGroup>().HasData(
            new DbEventTypeGroup { Id = "matter", Name = "Matter Events", Description = "Events related to legal matters" },
            new DbEventTypeGroup { Id = "document", Name = "Document Events", Description = "Events related to documents" },
            new DbEventTypeGroup { Id = "communication", Name = "Communication Events", Description = "Events related to communications" },
            new DbEventTypeGroup { Id = "billing", Name = "Billing Events", Description = "Events related to billing and finances" }
        );

        // Seed event types
        modelBuilder.Entity<DbEventType>().HasData(
            new DbEventType { Id = "matter.created", Name = "Matter Created", Description = "A new legal matter was created", GroupId = "matter" },
            new DbEventType { Id = "matter.updated", Name = "Matter Updated", Description = "A legal matter was updated", GroupId = "matter" },
            new DbEventType { Id = "matter.closed", Name = "Matter Closed", Description = "A legal matter was closed", GroupId = "matter" },
            new DbEventType { Id = "document.uploaded", Name = "Document Uploaded", Description = "A document was uploaded", GroupId = "document" },
            new DbEventType { Id = "document.reviewed", Name = "Document Reviewed", Description = "A document was reviewed", GroupId = "document" },
            new DbEventType { Id = "communication.email", Name = "Email Sent", Description = "An email was sent", GroupId = "communication" },
            new DbEventType { Id = "communication.call", Name = "Phone Call", Description = "A phone call was made", GroupId = "communication" },
            new DbEventType { Id = "billing.invoice", Name = "Invoice Generated", Description = "An invoice was generated", GroupId = "billing" },
            new DbEventType { Id = "billing.payment", Name = "Payment Received", Description = "A payment was received", GroupId = "billing" }
        );

        // Seed legal matter categories
        modelBuilder.Entity<DbLegalMatterCategory>().HasData(
            new DbLegalMatterCategory { Id = "corporate", Name = "Corporate Law" },
            new DbLegalMatterCategory { Id = "litigation", Name = "Litigation" },
            new DbLegalMatterCategory { Id = "employment", Name = "Employment Law" },
            new DbLegalMatterCategory { Id = "intellectual-property", Name = "Intellectual Property" },
            new DbLegalMatterCategory { Id = "real-estate", Name = "Real Estate" },
            new DbLegalMatterCategory { Id = "tax", Name = "Tax Law" },
            new DbLegalMatterCategory { Id = "criminal", Name = "Criminal Law" },
            new DbLegalMatterCategory { Id = "family", Name = "Family Law" }
        );

        // Seed some sample people
        modelBuilder.Entity<DbPerson>().HasData(
            new DbPerson { Id = "1", FirstName = "John", LastName = "Doe", FullName = "John Doe", Initials = "JD", HasPicture = false, PictureUrl = null },
            new DbPerson { Id = "2", FirstName = "Jane", LastName = "Smith", FullName = "Jane Smith", Initials = "JS", HasPicture = false, PictureUrl = null },
            new DbPerson { Id = "3", FirstName = "Robert", LastName = "Johnson", FullName = "Robert Johnson", Initials = "RJ", HasPicture = false, PictureUrl = null },
            new DbPerson { Id = "4", FirstName = "Emily", LastName = "Davis", FullName = "Emily Davis", Initials = "ED", HasPicture = false, PictureUrl = null },
            new DbPerson { Id = "5", FirstName = "Michael", LastName = "Wilson", FullName = "Michael Wilson", Initials = "MW", HasPicture = false, PictureUrl = null }
        );
    }
}
