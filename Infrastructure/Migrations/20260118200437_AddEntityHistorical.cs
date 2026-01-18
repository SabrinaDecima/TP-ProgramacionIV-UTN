using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityHistorical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Historicals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GymClassId = table.Column<int>(type: "int", nullable: false),
                    ClassDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historicals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historicals_GymClasses_GymClassId",
                        column: x => x.GymClassId,
                        principalTable: "GymClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Historicals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEH13Z14IdpKTs0lAciPjEH68V5utaF7JlEddnng8vsh/GD081noBroI0gfAptt7eLA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEJi7bz6YE8D46GMlho74KYtok+lpjm40WtguxRQF3+S0AMiJ4hwU7SHNnGeYdaRjvA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEEV9lsMZxMBxWh4R6JMmYv6py8+bQhINumKqoKPJ4qeKLS+U6gvsNwZ5HahPuqa98Q==");

            migrationBuilder.CreateIndex(
                name: "IX_Historicals_GymClassId",
                table: "Historicals",
                column: "GymClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Historicals_UserId",
                table: "Historicals",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historicals");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEMH836vbDzdRC+mF5LmWID/hLBPismUpNPAnW7bST643ysQs18TXdUQB4G8YpgwX5A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEJJYXPms2PL8OOqlPcDMfwAOyRXc2BERlY0YUj0DrBgBpL4awX47f7+P20R52iOmtg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEKr76gZU3odjBzWpgGFPOGAYZ7puDJO8W38lVdzBeo+kLWS7hnxVDGr1LSiNbo9EDQ==");
        }
    }
}
