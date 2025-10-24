using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceFechaWithDiaEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "GymClasses");

            migrationBuilder.AddColumn<int>(
                name: "Dia",
                table: "GymClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dia",
                table: "GymClasses");

            migrationBuilder.AddColumn<string>(
                name: "Fecha",
                table: "GymClasses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
