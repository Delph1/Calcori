using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calcori.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserMeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalCalories = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMeals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserMealId = table.Column<int>(type: "INTEGER", nullable: false),
                    FoodName = table.Column<string>(type: "TEXT", nullable: false),
                    CaloriesPer100g = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealItems_UserMeals_UserMealId",
                        column: x => x.UserMealId,
                        principalTable: "UserMeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealItems_UserMealId",
                table: "MealItems",
                column: "UserMealId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealItems");

            migrationBuilder.DropTable(
                name: "UserMeals");
        }
    }
}
