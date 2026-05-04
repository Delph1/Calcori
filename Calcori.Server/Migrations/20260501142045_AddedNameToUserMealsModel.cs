using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calcori.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedNameToUserMealsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserMeals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserMeals");
        }
    }
}
