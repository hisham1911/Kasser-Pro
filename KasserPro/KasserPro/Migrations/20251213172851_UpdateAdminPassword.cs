using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KasserPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$2aKbcflBGE.mC9RZwgYX3eBqlFqjkY2w8omg9m1mWkprp.cOl0dkG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$5EqkFvM3Y8LZZ0YHqVn4ZeOxNc7DWYpCvLZU3p6qWGNJxJ8HK9EzS");
        }
    }
}
