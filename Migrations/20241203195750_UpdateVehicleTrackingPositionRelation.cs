using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SincronizacionCoordenadas.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicleTrackingPositionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Positions_VehicleTrackingId",
                table: "Positions");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_VehicleTrackingId",
                table: "Positions",
                column: "VehicleTrackingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Positions_VehicleTrackingId",
                table: "Positions");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_VehicleTrackingId",
                table: "Positions",
                column: "VehicleTrackingId",
                unique: true);
        }
    }
}
