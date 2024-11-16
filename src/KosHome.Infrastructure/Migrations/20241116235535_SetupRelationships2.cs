using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KosHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetupRelationships2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_apartment_images_ApartmentId",
                table: "apartment_images",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_apartment_images_apartments_ApartmentId",
                table: "apartment_images",
                column: "ApartmentId",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apartment_images_apartments_ApartmentId",
                table: "apartment_images");

            migrationBuilder.DropIndex(
                name: "IX_apartment_images_ApartmentId",
                table: "apartment_images");
        }
    }
}
