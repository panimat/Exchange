using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DBRepository.Migrations
{
    public partial class UpdateRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_LastUpdates_DateUpdateId",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_DateUpdateId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "DateUpdateId",
                table: "Rates");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdate",
                table: "Rates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdate",
                table: "Rates");

            migrationBuilder.AddColumn<int>(
                name: "DateUpdateId",
                table: "Rates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rates_DateUpdateId",
                table: "Rates",
                column: "DateUpdateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_LastUpdates_DateUpdateId",
                table: "Rates",
                column: "DateUpdateId",
                principalTable: "LastUpdates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
