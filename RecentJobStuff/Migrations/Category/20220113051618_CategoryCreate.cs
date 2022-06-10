using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecentJobStuff.Migrations.Category
{
    public partial class CategoryCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UrlId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UrlId",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
