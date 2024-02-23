using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configuration.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ServicesActions");

            migrationBuilder.AddColumn<string>(
                name: "ActionName",
                table: "ServicesActions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesActions_ActionName",
                table: "ServicesActions",
                column: "ActionName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServicesActions_ActionName",
                table: "ServicesActions");

            migrationBuilder.DropColumn(
                name: "ActionName",
                table: "ServicesActions");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ServicesActions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
