using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsUserLastLoginOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastLoginOn",
                schema: "identity",
                table: "FgsUser",
                type: "timestamptz",
                nullable: true,
                comment: "UTC timestamp of the user's most recent successful login.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginOn",
                schema: "identity",
                table: "FgsUser");
        }
    }
}
