using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResturantProj.Migrations
{
    /// <inheritdoc />
    public partial class anotherone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_IsDeleted",
                table: "MenuItems",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MenuItems_IsDeleted",
                table: "MenuItems");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
