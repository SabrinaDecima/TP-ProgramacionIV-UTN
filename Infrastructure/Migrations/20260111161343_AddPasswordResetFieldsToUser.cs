using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Contraseña", "PasswordResetToken", "ResetTokenExpires" },
                values: new object[] { "AQAAAAIAAYagAAAAEEmW5ouzD8BInVDavG2ikrkavjYHzgVGH3KAHFrtTWky1LQtmZffal5+5yFJn8OLzA==", null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Contraseña", "PasswordResetToken", "ResetTokenExpires" },
                values: new object[] { "AQAAAAIAAYagAAAAEGqNYsezYFiHrzkNTDNVnvfe1WBCbIFUU7KW9kw0rZABCSd36BR6PmL8Gj3P7HiMDQ==", null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Contraseña", "PasswordResetToken", "ResetTokenExpires" },
                values: new object[] { "AQAAAAIAAYagAAAAEIgdgqQxapbClVrJJ2U8RPqg6WT/JA0AeJrA0ZU6LIXEI1cLjF3/PVgtIXF/O+EGWw==", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEKAoOfqLoo+V+gs7ZwKAw9pMR4l/cMmprMfj2HJuftLA9le4wIFO3Xb5WYDsO8G6Ig==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEAcrvjVIE7adzR93Y0OAqfNqTF6zJ5X7wRpG3yrdV9m62g2effgWh8JLGjt7/dWtsg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEBaAnD8I8MBKmQuxK9yvlowga+F/0FEy3qp1O5knerPLvonIE2qWjJl1+dudCFbFLQ==");
        }
    }
}
