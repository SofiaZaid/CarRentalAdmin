using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalAdministration.DAL.Migrations
{
    public partial class decimal_precision_on_booking_price : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCostOfRent",
                table: "Bookings",
                type: "decimal(10,5)",
                precision: 10,
                scale: 5,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCostOfRent",
                table: "Bookings",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,5)",
                oldPrecision: 10,
                oldScale: 5,
                oldNullable: true);
        }
    }
}
