using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoseidonPool.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image1Key",
                table: "ProductImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2Key",
                table: "ProductImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3Key",
                table: "ProductImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image4Key",
                table: "ProductImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image5Key",
                table: "ProductImages",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1Key",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Image2Key",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Image3Key",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Image4Key",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Image5Key",
                table: "ProductImages");
        }
    }
}
