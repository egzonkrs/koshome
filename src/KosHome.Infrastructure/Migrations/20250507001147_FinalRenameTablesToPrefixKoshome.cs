using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalRenameTablesToPrefixKoshome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apartments_koshome_cities_LocationId",
                table: "apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_apartments_property_types_PropertyTypeId",
                table: "apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_apartments_users_UserId",
                table: "apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_koshome_apartment_images_apartments_ApartmentId",
                table: "koshome_apartment_images");

            migrationBuilder.DropForeignKey(
                name: "FK_koshome_cities_countries_CountryId",
                table: "koshome_cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_property_types",
                table: "property_types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_countries",
                table: "countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_apartments",
                table: "apartments");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "koshome_users");

            migrationBuilder.RenameTable(
                name: "property_types",
                newName: "koshome_property_types");

            migrationBuilder.RenameTable(
                name: "countries",
                newName: "koshome_countries");

            migrationBuilder.RenameTable(
                name: "apartments",
                newName: "koshome_apartments");

            migrationBuilder.RenameIndex(
                name: "IX_users_IdentityId",
                table: "koshome_users",
                newName: "IX_koshome_users_IdentityId");

            migrationBuilder.RenameIndex(
                name: "IX_users_Email",
                table: "koshome_users",
                newName: "IX_koshome_users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_property_types_Name",
                table: "koshome_property_types",
                newName: "IX_koshome_property_types_Name");

            migrationBuilder.RenameIndex(
                name: "IX_countries_Alpha3Code",
                table: "koshome_countries",
                newName: "IX_koshome_countries_Alpha3Code");

            migrationBuilder.RenameIndex(
                name: "IX_apartments_UserId",
                table: "koshome_apartments",
                newName: "IX_koshome_apartments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_apartments_PropertyTypeId",
                table: "koshome_apartments",
                newName: "IX_koshome_apartments_PropertyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_apartments_LocationId",
                table: "koshome_apartments",
                newName: "IX_koshome_apartments_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_koshome_users",
                table: "koshome_users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_koshome_property_types",
                table: "koshome_property_types",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_koshome_countries",
                table: "koshome_countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_koshome_apartments",
                table: "koshome_apartments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_koshome_apartment_images_koshome_apartments_ApartmentId",
                table: "koshome_apartment_images",
                column: "ApartmentId",
                principalTable: "koshome_apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_koshome_apartments_koshome_cities_LocationId",
                table: "koshome_apartments",
                column: "LocationId",
                principalTable: "koshome_cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_koshome_apartments_koshome_property_types_PropertyTypeId",
                table: "koshome_apartments",
                column: "PropertyTypeId",
                principalTable: "koshome_property_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_koshome_apartments_koshome_users_UserId",
                table: "koshome_apartments",
                column: "UserId",
                principalTable: "koshome_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_koshome_cities_koshome_countries_CountryId",
                table: "koshome_cities",
                column: "CountryId",
                principalTable: "koshome_countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_koshome_apartment_images_koshome_apartments_ApartmentId",
                table: "koshome_apartment_images");

            migrationBuilder.DropForeignKey(
                name: "FK_koshome_apartments_koshome_cities_LocationId",
                table: "koshome_apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_koshome_apartments_koshome_property_types_PropertyTypeId",
                table: "koshome_apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_koshome_apartments_koshome_users_UserId",
                table: "koshome_apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_koshome_cities_koshome_countries_CountryId",
                table: "koshome_cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_koshome_users",
                table: "koshome_users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_koshome_property_types",
                table: "koshome_property_types");

            migrationBuilder.DropPrimaryKey(
                name: "PK_koshome_countries",
                table: "koshome_countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_koshome_apartments",
                table: "koshome_apartments");

            migrationBuilder.RenameTable(
                name: "koshome_users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "koshome_property_types",
                newName: "property_types");

            migrationBuilder.RenameTable(
                name: "koshome_countries",
                newName: "countries");

            migrationBuilder.RenameTable(
                name: "koshome_apartments",
                newName: "apartments");

            migrationBuilder.RenameIndex(
                name: "IX_koshome_users_IdentityId",
                table: "users",
                newName: "IX_users_IdentityId");

            migrationBuilder.RenameIndex(
                name: "IX_koshome_users_Email",
                table: "users",
                newName: "IX_users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_koshome_property_types_Name",
                table: "property_types",
                newName: "IX_property_types_Name");

            migrationBuilder.RenameIndex(
                name: "IX_koshome_countries_Alpha3Code",
                table: "countries",
                newName: "IX_countries_Alpha3Code");

            migrationBuilder.RenameIndex(
                name: "IX_koshome_apartments_UserId",
                table: "apartments",
                newName: "IX_apartments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_koshome_apartments_PropertyTypeId",
                table: "apartments",
                newName: "IX_apartments_PropertyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_koshome_apartments_LocationId",
                table: "apartments",
                newName: "IX_apartments_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_property_types",
                table: "property_types",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_countries",
                table: "countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_apartments",
                table: "apartments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_apartments_koshome_cities_LocationId",
                table: "apartments",
                column: "LocationId",
                principalTable: "koshome_cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_apartments_property_types_PropertyTypeId",
                table: "apartments",
                column: "PropertyTypeId",
                principalTable: "property_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_apartments_users_UserId",
                table: "apartments",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
