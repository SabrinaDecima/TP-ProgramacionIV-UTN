using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedGymClasses2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Hora", "ImageUrl" },
                values: new object[] { "08:00", "https://placehold.co/600x400?text=Yoga&font=montserrat" });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Descripcion", "Dia", "DuracionMinutos", "Hora", "ImageUrl", "Nombre" },
                values: new object[] { "Clase de relajación y estiramiento", 1, 60, "18:00", "https://placehold.co/600x400?text=Yoga&font=montserrat", "Yoga" });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descripcion", "DuracionMinutos", "Hora", "ImageUrl", "MaxCapacityUser", "Nombre" },
                values: new object[] { "Clase de relajación y estiramiento", 60, "08:00", "https://placehold.co/600x400?text=Yoga&font=montserrat", 3, "Yoga" });

            migrationBuilder.InsertData(
                table: "GymClasses",
                columns: new[] { "Id", "Descripcion", "Dia", "DuracionMinutos", "Hora", "ImageUrl", "MaxCapacityUser", "Nombre" },
                values: new object[,]
                {
                    { 4, "Clase de relajación y estiramiento", 3, 60, "19:00", "https://placehold.co/600x400?text=Yoga&font=montserrat", 3, "Yoga" },
                    { 5, "Clase de relajación y estiramiento", 5, 60, "08:00", "https://placehold.co/600x400?text=Yoga&font=montserrat", 3, "Yoga" },
                    { 6, "Clase de relajación y estiramiento", 5, 60, "17:00", "https://placehold.co/600x400?text=Yoga&font=montserrat", 3, "Yoga" },
                    { 7, "Entrenamiento funcional de alta intensidad", 2, 45, "07:00", "https://placehold.co/600x400?text=CrossFit&font=montserrat", 3, "CrossFit" },
                    { 8, "Entrenamiento funcional de alta intensidad", 2, 45, "19:00", "https://placehold.co/600x400?text=CrossFit&font=montserrat", 3, "CrossFit" },
                    { 9, "Entrenamiento funcional de alta intensidad", 4, 45, "07:00", "https://placehold.co/600x400?text=CrossFit&font=montserrat", 3, "CrossFit" },
                    { 10, "Entrenamiento funcional de alta intensidad", 4, 45, "20:00", "https://placehold.co/600x400?text=CrossFit&font=montserrat", 3, "CrossFit" },
                    { 11, "Entrenamiento funcional de alta intensidad", 6, 45, "09:00", "https://placehold.co/600x400?text=CrossFit&font=montserrat", 3, "CrossFit" },
                    { 12, "Entrenamiento funcional de alta intensidad", 6, 45, "11:00", "https://placehold.co/600x400?text=CrossFit&font=montserrat", 3, "CrossFit" },
                    { 13, "Ejercicio cardiovascular en bicicleta", 1, 50, "20:00", "https://placehold.co/600x400?text=Spinning&font=montserrat", 15, "Spinning" },
                    { 14, "Ejercicio cardiovascular en bicicleta", 2, 50, "20:00", "https://placehold.co/600x400?text=Spinning&font=montserrat", 15, "Spinning" },
                    { 15, "Ejercicio cardiovascular en bicicleta", 3, 50, "20:00", "https://placehold.co/600x400?text=Spinning&font=montserrat", 15, "Spinning" },
                    { 16, "Ejercicio cardiovascular en bicicleta", 4, 50, "20:00", "https://placehold.co/600x400?text=Spinning&font=montserrat", 15, "Spinning" },
                    { 17, "Ejercicio cardiovascular en bicicleta", 5, 50, "20:00", "https://placehold.co/600x400?text=Spinning&font=montserrat", 15, "Spinning" },
                    { 18, "Ejercicio cardiovascular en bicicleta", 6, 50, "18:00", "https://placehold.co/600x400?text=Spinning&font=montserrat", 15, "Spinning" },
                    { 19, "Fortalecimiento y control postural", 2, 55, "18:00", "https://placehold.co/600x400?text=Pilates&font=montserrat", 5, "Pilates" },
                    { 20, "Fortalecimiento y control postural", 4, 55, "18:00", "https://placehold.co/600x400?text=Pilates&font=montserrat", 5, "Pilates" },
                    { 21, "Fortalecimiento y control postural", 6, 55, "10:00", "https://placehold.co/600x400?text=Pilates&font=montserrat", 5, "Pilates" },
                    { 22, "Técnica, sacos y condición física", 1, 60, "19:00", "https://placehold.co/600x400?text=Boxeo&font=montserrat", 8, "Boxeo" },
                    { 23, "Técnica, sacos y condición física", 3, 60, "19:00", "https://placehold.co/600x400?text=Boxeo&font=montserrat", 8, "Boxeo" },
                    { 24, "Técnica, sacos y condición física", 5, 60, "19:00", "https://placehold.co/600x400?text=Boxeo&font=montserrat", 8, "Boxeo" },
                    { 25, "Baile y ritmo latino para quemar calorías", 2, 50, "20:30", "https://placehold.co/600x400?text=Zumba&font=montserrat", 20, "Zumba" },
                    { 26, "Baile y ritmo latino para quemar calorías", 4, 50, "20:30", "https://placehold.co/600x400?text=Zumba&font=montserrat", 20, "Zumba" },
                    { 27, "Baile y ritmo latino para quemar calorías", 6, 50, "12:00", "https://placehold.co/600x400?text=Zumba&font=montserrat", 20, "Zumba" }
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 7);

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
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 15);

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

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Hora", "ImageUrl" },
                values: new object[] { "18:00", "images/yoga.jpg" });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Descripcion", "Dia", "DuracionMinutos", "Hora", "ImageUrl", "Nombre" },
                values: new object[] { "Entrenamiento funcional de alta intensidad", 2, 45, "19:00", "images/crossfit.jpg", "CrossFit" });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descripcion", "DuracionMinutos", "Hora", "ImageUrl", "MaxCapacityUser", "Nombre" },
                values: new object[] { "Ejercicio cardiovascular en bicicleta", 50, "20:00", "images/spinning.jpg", 15, "Spinning" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEEmW5ouzD8BInVDavG2ikrkavjYHzgVGH3KAHFrtTWky1LQtmZffal5+5yFJn8OLzA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEGqNYsezYFiHrzkNTDNVnvfe1WBCbIFUU7KW9kw0rZABCSd36BR6PmL8Gj3P7HiMDQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEIgdgqQxapbClVrJJ2U8RPqg6WT/JA0AeJrA0ZU6LIXEI1cLjF3/PVgtIXF/O+EGWw==");
        }
    }
}
