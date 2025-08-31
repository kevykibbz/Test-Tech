using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddLegalMatterFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractType",
                table: "Matter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ContractValue",
                table: "Matter",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Matter",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Matter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "Matter",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Matter",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoverningLaw",
                table: "Matter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Matter",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Parties",
                table: "Matter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Matter",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractType",
                table: "Matter");

            migrationBuilder.DropColumn(
                name: "ContractValue",
                table: "Matter");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Matter");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Matter");

            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "Matter");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Matter");

            migrationBuilder.DropColumn(
                name: "GoverningLaw",
                table: "Matter");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Matter");

            migrationBuilder.DropColumn(
                name: "Parties",
                table: "Matter");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Matter");
        }
    }
}
