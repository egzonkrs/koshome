using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToPrefixKoshome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apartment_images_apartments_ApartmentId",
                table: "apartment_images");

            migrationBuilder.DropForeignKey(
                name: "FK_apartments_citites_LocationId",
                table: "apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_citites_countries_CountryId",
                table: "citites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_citites",
                table: "citites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_apartment_images",
                table: "apartment_images");

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

            migrationBuilder.RenameTable(
                name: "citites",
                newName: "koshome_cities");

            migrationBuilder.RenameTable(
                name: "apartment_images",
                newName: "koshome_apartment_images");

            migrationBuilder.RenameIndex(
                name: "IX_citites_CountryId",
                table: "koshome_cities",
                newName: "IX_koshome_cities_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_apartment_images_ApartmentId",
                table: "koshome_apartment_images",
                newName: "IX_koshome_apartment_images_ApartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_koshome_cities",
                table: "koshome_cities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_koshome_apartment_images",
                table: "koshome_apartment_images",
                column: "Id");

            migrationBuilder.InsertData(
                table: "property_types",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { "01JTM0Z8Q34CH3N4WRSFS10730", new DateTime(2025, 5, 7, 0, 5, 19, 203, DateTimeKind.Utc).AddTicks(8179), "A residential house.", "House", new DateTime(2025, 5, 7, 0, 5, 19, 203, DateTimeKind.Utc).AddTicks(8179) },
                    { "01JTM0Z8Q3QQKE2A6KRWE7QBBZ", new DateTime(2025, 5, 7, 0, 5, 19, 203, DateTimeKind.Utc).AddTicks(8180), "A commercial property.", "Commercial", new DateTime(2025, 5, 7, 0, 5, 19, 203, DateTimeKind.Utc).AddTicks(8181) },
                    { "01JTM0Z8Q3RNX4JWAQAHHXQJ8C", new DateTime(2025, 5, 7, 0, 5, 19, 203, DateTimeKind.Utc).AddTicks(8177), "A residential apartment.", "Apartment", new DateTime(2025, 5, 7, 0, 5, 19, 203, DateTimeKind.Utc).AddTicks(8178) },
                    { "01JTM0Z8Q3SNXR6TTHKYS44ZRB", new DateTime(2025, 5, 7, 0, 5, 19, 203, DateTimeKind.Utc).AddTicks(8182), "A land plot.", "Land", new DateTime(2025, 5, 7, 0, 5, 19, 203, DateTimeKind.Utc).AddTicks(8182) }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_apartments_koshome_cities_LocationId",
                table: "apartments",
                column: "LocationId",
                principalTable: "koshome_cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_koshome_apartment_images_apartments_ApartmentId",
                table: "koshome_apartment_images",
                column: "ApartmentId",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_koshome_cities_countries_CountryId",
                table: "koshome_cities",
                column: "CountryId",
                principalTable: "countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apartments_koshome_cities_LocationId",
                table: "apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_koshome_apartment_images_apartments_ApartmentId",
                table: "koshome_apartment_images");

            migrationBuilder.DropForeignKey(
                name: "FK_koshome_cities_countries_CountryId",
                table: "koshome_cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_koshome_cities",
                table: "koshome_cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_koshome_apartment_images",
                table: "koshome_apartment_images");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTM0Z8Q34CH3N4WRSFS10730");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTM0Z8Q3QQKE2A6KRWE7QBBZ");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTM0Z8Q3RNX4JWAQAHHXQJ8C");

            migrationBuilder.DeleteData(
                table: "property_types",
                keyColumn: "Id",
                keyValue: "01JTM0Z8Q3SNXR6TTHKYS44ZRB");

            migrationBuilder.RenameTable(
                name: "koshome_cities",
                newName: "citites");

            migrationBuilder.RenameTable(
                name: "koshome_apartment_images",
                newName: "apartment_images");

            migrationBuilder.RenameIndex(
                name: "IX_koshome_cities_CountryId",
                table: "citites",
                newName: "IX_citites_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_koshome_apartment_images_ApartmentId",
                table: "apartment_images",
                newName: "IX_apartment_images_ApartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_citites",
                table: "citites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_apartment_images",
                table: "apartment_images",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_apartment_images_apartments_ApartmentId",
                table: "apartment_images",
                column: "ApartmentId",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_apartments_citites_LocationId",
                table: "apartments",
                column: "LocationId",
                principalTable: "citites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_citites_countries_CountryId",
                table: "citites",
                column: "CountryId",
                principalTable: "countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
