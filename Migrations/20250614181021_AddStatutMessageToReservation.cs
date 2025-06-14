using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStatutMessageToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatutMessage",
                table: "Reservations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatutMessage",
                table: "Reservations");
        }
    }
}
