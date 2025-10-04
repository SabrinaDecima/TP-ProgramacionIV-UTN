using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovingInstructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GymClasses_Instructors_InstructorId",
                table: "GymClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_GymClasses_Instructors_InstructorId1",
                table: "GymClasses");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_GymClasses_InstructorId",
                table: "GymClasses");

            migrationBuilder.DropIndex(
                name: "IX_GymClasses_InstructorId1",
                table: "GymClasses");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "GymClasses");

            migrationBuilder.DropColumn(
                name: "InstructorId1",
                table: "GymClasses");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Hora",
                table: "GymClasses",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hora",
                table: "GymClasses");

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "GymClasses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstructorId1",
                table: "GymClasses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Apellido = table.Column<string>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GymClasses_InstructorId",
                table: "GymClasses",
                column: "InstructorId");

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
    }
}
