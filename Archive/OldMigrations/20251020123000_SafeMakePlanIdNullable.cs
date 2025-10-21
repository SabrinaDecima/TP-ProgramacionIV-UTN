using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SafeMakePlanIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) A�adir columna MaxCapacity (esta operaci�n es segura en SQLite)
            migrationBuilder.AddColumn<int>(
                name: "MaxCapacity",
                table: "GymClasses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 3);

            // 2) Rebuild seguro de la tabla Users para cambiar PlanId a nullable.
            //    Ajusta la lista de columnas si tu tabla Users tiene m�s columnas.
            migrationBuilder.Sql(@"
PRAGMA foreign_keys = OFF;

CREATE TABLE ""Users_temp"" (
    ""Id"" INTEGER NOT NULL PRIMARY KEY,
    ""Nombre"" TEXT NOT NULL,
    ""Apellido"" TEXT NOT NULL,
    ""Email"" TEXT NOT NULL,
    ""Telefono"" TEXT NOT NULL,
    ""Contrase�a"" TEXT NOT NULL,
    ""RoleId"" INTEGER NOT NULL,
    ""PlanId"" INTEGER
);

INSERT INTO ""Users_temp"" (""Id"", ""Nombre"", ""Apellido"", ""Email"", ""Telefono"", ""Contrase�a"", ""RoleId"", ""PlanId"")
SELECT ""Id"", ""Nombre"", ""Apellido"", ""Email"", ""Telefono"", ""Contrase�a"", ""RoleId"", ""PlanId"" FROM ""Users"";

DROP TABLE ""Users"";

ALTER TABLE ""Users_temp"" RENAME TO ""Users"";

PRAGMA foreign_keys = ON;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir el cambio de nullabilidad: reconstruimos la tabla para hacer PlanId NOT NULL
            // Los valores NULL en PlanId se convertir�n a 0 (ajusta seg�n tu l�gica de negocio).
            migrationBuilder.Sql(@"
PRAGMA foreign_keys = OFF;

CREATE TABLE ""Users_temp"" (
    ""Id"" INTEGER NOT NULL PRIMARY KEY,
    ""Nombre"" TEXT NOT NULL,
    ""Apellido"" TEXT NOT NULL,
    ""Email"" TEXT NOT NULL,
    ""Telefono"" TEXT NOT NULL,
    ""Contrase�a"" TEXT NOT NULL,
    ""RoleId"" INTEGER NOT NULL,
    ""PlanId"" INTEGER NOT NULL DEFAULT 0
);

INSERT INTO ""Users_temp"" (""Id"", ""Nombre"", ""Apellido"", ""Email"", ""Telefono"", ""Contrase�a"", ""RoleId"", ""PlanId"")
SELECT ""Id"", ""Nombre"", ""Apellido"", ""Email"", ""Telefono"", ""Contrase�a"", ""RoleId"", COALESCE(""PlanId"", 0) FROM ""Users"";

DROP TABLE ""Users"";

ALTER TABLE ""Users_temp"" RENAME TO ""Users"";

PRAGMA foreign_keys = ON;
");

            // Y eliminar la columna MaxCapacity de GymClasses.
            // En SQLite EF Core simula DropColumn; aqu� usamos DropColumn (EF lo adaptar�).
            migrationBuilder.DropColumn(
                name: "MaxCapacity",
                table: "GymClasses");
        }
    }
}