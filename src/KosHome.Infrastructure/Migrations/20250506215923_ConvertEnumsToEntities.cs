using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertEnumsToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListingType",
                table: "apartments");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "apartments");

            migrationBuilder.AddColumn<string>(
                name: "ListingTypeId",
                table: "apartments",
                type: "character varying(26)",
                maxLength: 26,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyTypeId",
                table: "apartments",
                type: "character varying(26)",
                maxLength: 26,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "listing_types",
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
                    table.PrimaryKey("PK_listing_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "property_types",
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
                    table.PrimaryKey("PK_property_types", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "listing_types",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "01JTKSRNJ42ZV6J9NV8BJ92XST", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(790), "The property is for sale.", "Sale", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(791) },
                    { "01JTKSRNJ4W239QH3K5F4VC3PY", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(792), "The property is for rent.", "Rent", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(792) }
                });

            migrationBuilder.InsertData(
                table: "property_types",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "01JTKSRNJ4CXA58TMM1RCDMEEK", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(4000), "A land plot.", "Land", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(4001) },
                    { "01JTKSRNJ4RSZ4HYZBQD601AJJ", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(3999), "A commercial property.", "Commercial", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(3999) },
                    { "01JTKSRNJ4VZ0MGWJGWQXE19T5", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(3995), "A residential apartment.", "Apartment", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(3996) },
                    { "01JTKSRNJ4WFH2NK59NZ4BC2WR", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(3997), "A residential house.", "House", new DateTime(2025, 5, 6, 21, 59, 22, 948, DateTimeKind.Utc).AddTicks(3998) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_apartments_ListingTypeId",
                table: "apartments",
                column: "ListingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_apartments_PropertyTypeId",
                table: "apartments",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_listing_types_Name",
                table: "listing_types",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_property_types_Name",
                table: "property_types",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_apartments_listing_types_ListingTypeId",
                table: "apartments",
                column: "ListingTypeId",
                principalTable: "listing_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_apartments_property_types_PropertyTypeId",
                table: "apartments",
                column: "PropertyTypeId",
                principalTable: "property_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apartments_listing_types_ListingTypeId",
                table: "apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_apartments_property_types_PropertyTypeId",
                table: "apartments");

            migrationBuilder.DropTable(
                name: "listing_types");

            migrationBuilder.DropTable(
                name: "property_types");

            migrationBuilder.DropIndex(
                name: "IX_apartments_ListingTypeId",
                table: "apartments");

            migrationBuilder.DropIndex(
                name: "IX_apartments_PropertyTypeId",
                table: "apartments");

            migrationBuilder.DropColumn(
                name: "ListingTypeId",
                table: "apartments");

            migrationBuilder.DropColumn(
                name: "PropertyTypeId",
                table: "apartments");

            migrationBuilder.AddColumn<string>(
                name: "ListingType",
                table: "apartments",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                table: "apartments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
