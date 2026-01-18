using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyingSeedClassNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 18);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GymClasses",
                columns: new[] { "Id", "Descripcion", "Dia", "DuracionMinutos", "Hora", "ImageUrl", "MaxCapacityUser", "Nombre" },
                values: new object[,]
                {
                    { 2, "Clase de relajación y estiramiento", 1, 60, "18:00", "https://placehold.co/600x400?text=Yoga&font=montserrat", 3, "Yoga" },
                    { 4, "Clase de relajación y estiramiento", 3, 60, "19:00", "https://placehold.co/600x400?text=Yoga&font=montserrat", 3, "Yoga" },
                    { 6, "Clase de relajación y estiramiento", 5, 60, "17:00", "https://placehold.co/600x400?text=Yoga&font=montserrat", 3, "Yoga" },
                    { 8, "Entrenamiento funcional de alta intensidad", 2, 45, "19:00", "https://placehold.co/600x400?text=CrossFit&font=montserrat", 3, "CrossFit" },
                    { 9, "Entrenamiento funcional de alta intensidad", 4, 45, "07:00", "https://placehold.co/600x400?text=CrossFit&font=montserrat", 3, "CrossFit" },
                    { 11, "Entrenamiento funcional de alta intensidad", 6, 45, "09:00", "https://placehold.co/600x400?text=CrossFit&font=montserrat", 3, "CrossFit" },
                    { 16, "Ejercicio cardiovascular en bicicleta", 4, 50, "20:00", "https://placehold.co/600x400?text=Spinning&font=montserrat", 15, "Spinning" },
                    { 17, "Ejercicio cardiovascular en bicicleta", 5, 50, "20:00", "https://placehold.co/600x400?text=Spinning&font=montserrat", 15, "Spinning" },
                    { 18, "Ejercicio cardiovascular en bicicleta", 6, 50, "18:00", "https://placehold.co/600x400?text=Spinning&font=montserrat", 15, "Spinning" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEGcgHZBzkJyzCmzJJ5Pdsgag79UZFtiD+D1XULIXJofHI/dZ0NBxS5PBVIk3tSwCeg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEHr55677YyvbxlxG8FCwdiQXHQA/pqkYGZWiQ0EZQtvGRoO929SMzEL7pb9vRFHdoQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEPbUTnhM37g74/SS0FJ84lkyPR/NQUV9kxbzR9Ij4xpu+Id9iLHhHVydLvP1h2l1bw==");
        }
    }
}
