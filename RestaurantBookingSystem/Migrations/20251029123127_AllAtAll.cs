using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AllAtAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ManagerDetails_Email",
                table: "ManagerDetails");

            migrationBuilder.AddColumn<bool>(
                name: "AdvancePayment",
                table: "Reservation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "AdvancePaymentAmount",
                table: "Reservation",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "verification",
                table: "ManagerDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ManagerDetails",
                keyColumn: "ManagerId",
                keyValue: 1,
                column: "verification",
                value: "Unverified");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvancePayment",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "AdvancePaymentAmount",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "verification",
                table: "ManagerDetails");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerDetails_Email",
                table: "ManagerDetails",
                column: "Email",
                unique: true);
        }
    }
}
