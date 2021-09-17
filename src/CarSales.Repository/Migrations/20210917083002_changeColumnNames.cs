using Microsoft.EntityFrameworkCore.Migrations;

namespace CarSales.Repository.Migrations
{
    public partial class changeColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedSaleDate",
                table: "Cars",
                newName: "StartedSale");

            migrationBuilder.RenameColumn(
                name: "FinishedSaleDate",
                table: "Cars",
                newName: "FinishedSale");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(15)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedSale",
                table: "Cars",
                newName: "StartedSaleDate");

            migrationBuilder.RenameColumn(
                name: "FinishedSale",
                table: "Cars",
                newName: "FinishedSaleDate");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Clients",
                type: "NVARCHAR(15)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
