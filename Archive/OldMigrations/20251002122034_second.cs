using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GymClasses_Instructors_InstructorId",
                table: "GymClasses");

            migrationBuilder.DropTable(
                name: "Historicals");

            migrationBuilder.DropTable(
                name: "Reserves");

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Plans",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "GymClasses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "InstructorId1",
                table: "GymClasses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Nombre", "Precio" },
                values: new object[,]
                {
                    { 1, "Basic", 25.5m },
                    { 2, "Premium", 45.0m },
                    { 3, "Elite", 70.0m }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Socio" },
                    { 2, "Administrador" },
                    { 3, "SuperAdministrador" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Apellido", "Contraseña", "Email", "Nombre", "PlanId", "RoleId", "Telefono" },
                values: new object[,]
                {
                    { 1, "uno", "1234", "cliente@demo.com", "cliente", 1, 1, "1234" },
                    { 2, "demo", "1234", "admin@demo.com", "admin", 2, 2, "5678" },
                    { 3, "demo", "1234", "superadmin@demo.com", "superadmin", 3, 3, "9999" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GymClasses_InstructorId1",
                table: "GymClasses",
                column: "InstructorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_GymClasses_Instructors_InstructorId",
                table: "GymClasses",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GymClasses_Instructors_InstructorId1",
                table: "GymClasses",
                column: "InstructorId1",
                principalTable: "Instructors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GymClasses_Instructors_InstructorId",
                table: "GymClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_GymClasses_Instructors_InstructorId1",
                table: "GymClasses");

            migrationBuilder.DropIndex(
                name: "IX_GymClasses_InstructorId1",
                table: "GymClasses");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "GymClasses");

            migrationBuilder.DropColumn(
                name: "InstructorId1",
                table: "GymClasses");

            migrationBuilder.CreateTable(
                name: "Historicals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historicals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historicals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GymClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Asistio = table.Column<bool>(type: "INTEGER", nullable: false),
                    FechaReserva = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Pagado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserves_GymClasses_GymClassId",
                        column: x => x.GymClassId,
                        principalTable: "GymClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserves_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Historicals_UserId",
                table: "Historicals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_GymClassId",
                table: "Reserves",
                column: "GymClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_UserId",
                table: "Reserves",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GymClasses_Instructors_InstructorId",
                table: "GymClasses",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
