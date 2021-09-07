using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class CreateClientAndCarTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "Int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityNumber = table.Column<string>(type: "NVARCHAR(11)", nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    SecondName = table.Column<string>(type: "NVARCHAR(30)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(15)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    Address = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ClientId", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "Int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "Int", nullable: false),
                    Brand = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    Model = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    CarNumber = table.Column<string>(type: "NVARCHAR(15)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    VinCode = table.Column<string>(type: "VARCHAR(17)", nullable: false),
                    Price = table.Column<int>(type: "Int", nullable: false),
                    StartedSaleDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    FinishedSaleDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_CarId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Car_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Car_ClientId",
                table: "Car",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
