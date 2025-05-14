using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalizedDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "koshome_countries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    CountryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Alpha3Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_koshome_countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "koshome_property_types",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_koshome_property_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "koshome_users",
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
                    table.PrimaryKey("PK_koshome_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "koshome_cities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    CityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Alpha3Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CountryId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_koshome_cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_koshome_cities_koshome_countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "koshome_countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "koshome_apartments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    UserId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ListingType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PropertyTypeId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CityId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Bedrooms = table.Column<int>(type: "integer", nullable: false),
                    Bathrooms = table.Column<int>(type: "integer", nullable: false),
                    SquareMeters = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_koshome_apartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_koshome_apartments_koshome_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "koshome_cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_koshome_apartments_koshome_property_types_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "koshome_property_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_koshome_apartments_koshome_users_UserId",
                        column: x => x.UserId,
                        principalTable: "koshome_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "koshome_apartment_images",
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
                    table.PrimaryKey("PK_koshome_apartment_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_koshome_apartment_images_koshome_apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "koshome_apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_koshome_apartment_images_ApartmentId",
                table: "koshome_apartment_images",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_koshome_apartments_CityId",
                table: "koshome_apartments",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_koshome_apartments_PropertyTypeId",
                table: "koshome_apartments",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_koshome_apartments_UserId",
                table: "koshome_apartments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_koshome_cities_CountryId",
                table: "koshome_cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_koshome_countries_Alpha3Code",
                table: "koshome_countries",
                column: "Alpha3Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_koshome_property_types_Name",
                table: "koshome_property_types",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_koshome_users_Email",
                table: "koshome_users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_koshome_users_IdentityId",
                table: "koshome_users",
                column: "IdentityId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "koshome_apartment_images");

            migrationBuilder.DropTable(
                name: "koshome_apartments");

            migrationBuilder.DropTable(
                name: "koshome_cities");

            migrationBuilder.DropTable(
                name: "koshome_property_types");

            migrationBuilder.DropTable(
                name: "koshome_users");

            migrationBuilder.DropTable(
                name: "koshome_countries");
        }
    }
}
