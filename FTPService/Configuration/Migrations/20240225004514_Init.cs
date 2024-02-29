using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configuration.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServicesPermisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesPermisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FtpConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FtpConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FtpConfigurations_ServicesPermisions_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "ServicesPermisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FtpServicesActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FtpServicesActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FtpServicesActions_ServicesPermisions_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "ServicesPermisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FtpFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceActionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FtpFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FtpFiles_FtpServicesActions_ServiceActionId",
                        column: x => x.ServiceActionId,
                        principalTable: "FtpServicesActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FtpConfigurations_ServiceId",
                table: "FtpConfigurations",
                column: "ServiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FtpFiles_ServiceActionId",
                table: "FtpFiles",
                column: "ServiceActionId");

            migrationBuilder.CreateIndex(
                name: "IX_FtpServicesActions_ActionName",
                table: "FtpServicesActions",
                column: "ActionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FtpServicesActions_ServiceId",
                table: "FtpServicesActions",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesPermisions_ServiceName",
                table: "ServicesPermisions",
                column: "ServiceName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FtpConfigurations");

            migrationBuilder.DropTable(
                name: "FtpFiles");

            migrationBuilder.DropTable(
                name: "FtpServicesActions");

            migrationBuilder.DropTable(
                name: "ServicesPermisions");
        }
    }
}
