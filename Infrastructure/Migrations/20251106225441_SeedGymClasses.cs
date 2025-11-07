using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedGymClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GymClasses",
                columns: new[] { "Id", "Descripcion", "Dia", "DuracionMinutos", "Hora", "ImageUrl", "MaxCapacityUser", "Nombre" },
                values: new object[,]
                {
                    { 1, "Clase de relajación y estiramiento", 1, 60, "18:00", "images/yoga.jpg", 3, "Yoga" },
                    { 2, "Entrenamiento funcional de alta intensidad", 2, 45, "19:00", "images/crossfit.jpg", 3, "CrossFit" },
                    { 3, "Ejercicio cardiovascular en bicicleta", 3, 50, "20:00", "images/spinning.jpg", 15, "Spinning" }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEEFrn+6DEsPXMequCgERU8LSyQH+PM6u9VAX4QeamSwTRorp0iUyG2C+fATGCamRUw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAECFWEFBykZZUBoD/AcUva5R1SXLoj5fanna9RHynumHspcja0toTPOBiVjoyIyio+A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEFYbqoLLCCvlrq6jX9skP44TUv4LcBx0uHTxt+KVKzoWAFwCyOqMMMd5W+RHfhYSow==");
        }
    }
}
