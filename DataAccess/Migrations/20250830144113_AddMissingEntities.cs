using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DecimalDigits = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTypeGroup",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypeGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalMatterCategory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalMatterCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Initials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasPicture = table.Column<bool>(type: "bit", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventType_EventTypeGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "EventTypeGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "DecimalDigits", "Name", "Symbol" },
                values: new object[,]
                {
                    { "AUD", 2, "Australian Dollar", "A$" },
                    { "EUR", 2, "Euro", "€" },
                    { "GBP", 2, "British Pound", "£" },
                    { "NZD", 2, "New Zealand Dollar", "NZ$" },
                    { "USD", 2, "US Dollar", "$" }
                });

            migrationBuilder.InsertData(
                table: "EventTypeGroup",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { "billing", "Events related to billing and finances", "Billing Events" },
                    { "communication", "Events related to communications", "Communication Events" },
                    { "document", "Events related to documents", "Document Events" },
                    { "matter", "Events related to legal matters", "Matter Events" }
                });

            migrationBuilder.InsertData(
                table: "LegalMatterCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "corporate", "Corporate Law" },
                    { "criminal", "Criminal Law" },
                    { "employment", "Employment Law" },
                    { "family", "Family Law" },
                    { "intellectual-property", "Intellectual Property" },
                    { "litigation", "Litigation" },
                    { "real-estate", "Real Estate" },
                    { "tax", "Tax Law" }
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "FirstName", "FullName", "HasPicture", "Initials", "LastName", "PictureUrl" },
                values: new object[,]
                {
                    { "1", "John", "John Doe", false, "JD", "Doe", null },
                    { "2", "Jane", "Jane Smith", false, "JS", "Smith", null },
                    { "3", "Robert", "Robert Johnson", false, "RJ", "Johnson", null },
                    { "4", "Emily", "Emily Davis", false, "ED", "Davis", null },
                    { "5", "Michael", "Michael Wilson", false, "MW", "Wilson", null }
                });

            migrationBuilder.InsertData(
                table: "EventType",
                columns: new[] { "Id", "Description", "GroupId", "Name" },
                values: new object[,]
                {
                    { "billing.invoice", "An invoice was generated", "billing", "Invoice Generated" },
                    { "billing.payment", "A payment was received", "billing", "Payment Received" },
                    { "communication.call", "A phone call was made", "communication", "Phone Call" },
                    { "communication.email", "An email was sent", "communication", "Email Sent" },
                    { "document.reviewed", "A document was reviewed", "document", "Document Reviewed" },
                    { "document.uploaded", "A document was uploaded", "document", "Document Uploaded" },
                    { "matter.closed", "A legal matter was closed", "matter", "Matter Closed" },
                    { "matter.created", "A new legal matter was created", "matter", "Matter Created" },
                    { "matter.updated", "A legal matter was updated", "matter", "Matter Updated" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventType_GroupId",
                table: "EventType",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "EventType");

            migrationBuilder.DropTable(
                name: "LegalMatterCategory");

            migrationBuilder.DropTable(
                name: "LogEntry");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "EventTypeGroup");
        }
    }
}
