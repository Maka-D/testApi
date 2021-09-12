using Microsoft.EntityFrameworkCore.Migrations;

namespace CarSales.Repository.Migrations
{
    public partial class _ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Clients_IdentityNumber",
                table: "Clients",
                column: "IdentityNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_VinCode",
                table: "Cars",
                column: "VinCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_IdentityNumber",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Cars_VinCode",
                table: "Cars");
        }
    }
}
