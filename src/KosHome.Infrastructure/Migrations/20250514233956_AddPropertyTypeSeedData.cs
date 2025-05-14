using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyTypeSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "koshome_property_types",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "01JV8JPHSBA5XKHAPNJMRKTYRW", new DateTime(2025, 5, 14, 23, 39, 56, 587, DateTimeKind.Utc).AddTicks(4892), "A residential house.", "House", new DateTime(2025, 5, 14, 23, 39, 56, 587, DateTimeKind.Utc).AddTicks(4892) },
                    { "01JV8JPHSBCCVE15AA564PRCYV", new DateTime(2025, 5, 14, 23, 39, 56, 587, DateTimeKind.Utc).AddTicks(4893), "A commercial property.", "Commercial", new DateTime(2025, 5, 14, 23, 39, 56, 587, DateTimeKind.Utc).AddTicks(4893) },
                    { "01JV8JPHSBJ4EQPT79A8SE1TMY", new DateTime(2025, 5, 14, 23, 39, 56, 587, DateTimeKind.Utc).AddTicks(4890), "A residential apartment.", "Apartment", new DateTime(2025, 5, 14, 23, 39, 56, 587, DateTimeKind.Utc).AddTicks(4890) },
                    { "01JV8JPHSBT9JS8XM4RQFPD68G", new DateTime(2025, 5, 14, 23, 39, 56, 587, DateTimeKind.Utc).AddTicks(4894), "A land plot.", "Land", new DateTime(2025, 5, 14, 23, 39, 56, 587, DateTimeKind.Utc).AddTicks(4894) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "koshome_property_types",
                keyColumn: "Id",
                keyValue: "01JV8JPHSBA5XKHAPNJMRKTYRW");

            migrationBuilder.DeleteData(
                table: "koshome_property_types",
                keyColumn: "Id",
                keyValue: "01JV8JPHSBCCVE15AA564PRCYV");

            migrationBuilder.DeleteData(
                table: "koshome_property_types",
                keyColumn: "Id",
                keyValue: "01JV8JPHSBJ4EQPT79A8SE1TMY");

            migrationBuilder.DeleteData(
                table: "koshome_property_types",
                keyColumn: "Id",
                keyValue: "01JV8JPHSBT9JS8XM4RQFPD68G");
        }
    }
}
