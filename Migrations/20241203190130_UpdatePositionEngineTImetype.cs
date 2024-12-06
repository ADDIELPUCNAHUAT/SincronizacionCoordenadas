﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SincronizacionCoordenadas.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePositionEngineTImetype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "EngineTime",
                table: "Positions",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EngineTime",
                table: "Positions",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}