using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookingSeats_BookingId",
                table: "BookingSeats");

            migrationBuilder.DropIndex(
                name: "IX_BookingSeats_ShowSeatId",
                table: "BookingSeats");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeats_BookingId_ShowSeatId",
                table: "BookingSeats",
                columns: new[] { "BookingId", "ShowSeatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeats_ShowSeatId",
                table: "BookingSeats",
                column: "ShowSeatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookingSeats_BookingId_ShowSeatId",
                table: "BookingSeats");

            migrationBuilder.DropIndex(
                name: "IX_BookingSeats_ShowSeatId",
                table: "BookingSeats");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeats_BookingId",
                table: "BookingSeats",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeats_ShowSeatId",
                table: "BookingSeats",
                column: "ShowSeatId",
                unique: true);
        }
    }
}
