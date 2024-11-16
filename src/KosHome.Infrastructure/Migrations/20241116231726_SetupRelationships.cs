using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetupRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "apartments",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "apartments",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_citites_CountryId",
                table: "citites",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_apartments_LocationId",
                table: "apartments",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_apartments_UserId",
                table: "apartments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_apartments_citites_LocationId",
                table: "apartments",
                column: "LocationId",
                principalTable: "citites",
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
                name: "FK_citites_countries_CountryId",
                table: "citites",
                column: "CountryId",
                principalTable: "countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apartments_citites_LocationId",
                table: "apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_apartments_users_UserId",
                table: "apartments");

            migrationBuilder.DropForeignKey(
                name: "FK_citites_countries_CountryId",
                table: "citites");

            migrationBuilder.DropIndex(
                name: "IX_citites_CountryId",
                table: "citites");

            migrationBuilder.DropIndex(
                name: "IX_apartments_LocationId",
                table: "apartments");

            migrationBuilder.DropIndex(
                name: "IX_apartments_UserId",
                table: "apartments");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "apartments");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "apartments");
        }
    }
}
