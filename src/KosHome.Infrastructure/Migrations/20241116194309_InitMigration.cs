using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apartment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    UserId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Title_Value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description_Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Price_Value = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ListingType_Value = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PropertyType_Value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address_Value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LocationId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Bedrooms = table.Column<int>(type: "integer", nullable: false),
                    Bathrooms = table.Column<int>(type: "integer", nullable: false),
                    SquareMeters = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentImage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    ApartmentId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentImage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    CityName_Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Alpha3Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CountryId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    CountryName_Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Alpha3Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    IdentityId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Country_Alpha3Code",
                table: "Country",
                column: "Alpha3Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_IdentityId",
                table: "User",
                column: "IdentityId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apartment");

            migrationBuilder.DropTable(
                name: "ApartmentImage");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
