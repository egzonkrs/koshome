using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetupProjectStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "apartment_images",
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
                    table.PrimaryKey("PK_apartment_images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "apartments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    UserId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ListingType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PropertyType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LocationId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Bedrooms = table.Column<int>(type: "integer", nullable: false),
                    Bathrooms = table.Column<int>(type: "integer", nullable: false),
                    SquareMeters = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "citites",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    CityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Alpha3Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CountryId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    CountryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Alpha3Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
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
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_countries_Alpha3Code",
                table: "countries",
                column: "Alpha3Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_IdentityId",
                table: "users",
                column: "IdentityId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "apartment_images");

            migrationBuilder.DropTable(
                name: "apartments");

            migrationBuilder.DropTable(
                name: "citites");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
