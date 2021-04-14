using Microsoft.EntityFrameworkCore.Migrations;

namespace MyRecipes.Data.Migrations
{
    public partial class RemoveRemoteUrlProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemoteUrl",
                table: "Images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RemoteUrl",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
