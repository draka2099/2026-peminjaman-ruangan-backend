using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_peminjaman_ruangan_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "bookings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "bookings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "bookings");
        }
    }
}
