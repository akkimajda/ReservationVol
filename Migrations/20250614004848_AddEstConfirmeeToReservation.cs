using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEstConfirmeeToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstConfirmee",
                table: "Reservations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstConfirmee",
                table: "Reservations");
        }
    }
}
