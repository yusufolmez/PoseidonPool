using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoseidonPool.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Baskets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BasketId",
                table: "Baskets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
