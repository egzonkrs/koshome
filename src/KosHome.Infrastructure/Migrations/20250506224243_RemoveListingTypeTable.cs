using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveListingTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apartments_listing_types_ListingTypeId",
                table: "apartments");

            migrationBuilder.DropTable(
                name: "listing_types");

            migrationBuilder.DropIndex(
                name: "IX_apartments_ListingTypeId",
                table: "apartments");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTKSRNJ4CXA58TMM1RCDMEEK");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTKSRNJ4RSZ4HYZBQD601AJJ");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTKSRNJ4VZ0MGWJGWQXE19T5");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTKSRNJ4WFH2NK59NZ4BC2WR");

            migrationBuilder.DropColumn(
                name: "ListingTypeId",
                table: "apartments");

            migrationBuilder.AddColumn<string>(
                name: "ListingType",
                table: "apartments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "property_types",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "01JTKW80QR8RBSSS34TA1DPDCZ", new DateTime(2025, 5, 6, 22, 42, 43, 64, DateTimeKind.Utc).AddTicks(76), "A commercial property.", "Commercial", new DateTime(2025, 5, 6, 22, 42, 43, 64, DateTimeKind.Utc).AddTicks(76) },
                    { "01JTKW80QRD6J22QWYF3PEN7T7", new DateTime(2025, 5, 6, 22, 42, 43, 64, DateTimeKind.Utc).AddTicks(75), "A residential house.", "House", new DateTime(2025, 5, 6, 22, 42, 43, 64, DateTimeKind.Utc).AddTicks(75) },
                    { "01JTKW80QRKAHETZQ7B0REKPXD", new DateTime(2025, 5, 6, 22, 42, 43, 64, DateTimeKind.Utc).AddTicks(73), "A residential apartment.", "Apartment", new DateTime(2025, 5, 6, 22, 42, 43, 64, DateTimeKind.Utc).AddTicks(73) },
                    { "01JTKW80QRS6GPP7M5P1QDBNF8", new DateTime(2025, 5, 6, 22, 42, 43, 64, DateTimeKind.Utc).AddTicks(77), "A land plot.", "Land", new DateTime(2025, 5, 6, 22, 42, 43, 64, DateTimeKind.Utc).AddTicks(78) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTKW80QR8RBSSS34TA1DPDCZ");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTKW80QRD6J22QWYF3PEN7T7");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTKW80QRKAHETZQ7B0REKPXD");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTKW80QRS6GPP7M5P1QDBNF8");

            migrationBuilder.DropColumn(
                name: "ListingType",
                table: "apartments");

            migrationBuilder.AddColumn<string>(
                name: "ListingTypeId",
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
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_types", x => x.Id);
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
                name: "IX_listing_types_Name",
                table: "listing_types",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_apartments_listing_types_ListingTypeId",
                table: "apartments",
                column: "ListingTypeId",
                principalTable: "listing_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
