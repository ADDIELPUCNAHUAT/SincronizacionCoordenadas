using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SincronizacionCoordenadas.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleTrackings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imei = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastReportedTimeLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastReportedTimeUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTrackings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InputOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    VehicleTrackingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputOutputs_VehicleTrackings_VehicleTrackingId",
                        column: x => x.VehicleTrackingId,
                        principalTable: "VehicleTrackings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speed = table.Column<double>(type: "float", nullable: false),
                    SpeedMeasure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Heading = table.Column<int>(type: "int", nullable: false),
                    Ignition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Odometer = table.Column<double>(type: "float", nullable: false),
                    EngineTime = table.Column<int>(type: "int", nullable: false),
                    EngineStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerTimeUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GPSTimeLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GPSTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PointOfInterestUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointOfInterestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleTrackingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_VehicleTrackings_VehicleTrackingId",
                        column: x => x.VehicleTrackingId,
                        principalTable: "VehicleTrackings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitUid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadingTimeLocal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SensorType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleTrackingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorReadings_VehicleTrackings_VehicleTrackingId",
                        column: x => x.VehicleTrackingId,
                        principalTable: "VehicleTrackings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputOutputs_VehicleTrackingId",
                table: "InputOutputs",
                column: "VehicleTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_VehicleTrackingId",
                table: "Positions",
                column: "VehicleTrackingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_VehicleTrackingId",
                table: "SensorReadings",
                column: "VehicleTrackingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputOutputs");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "SensorReadings");

            migrationBuilder.DropTable(
                name: "VehicleTrackings");
        }
    }
}
