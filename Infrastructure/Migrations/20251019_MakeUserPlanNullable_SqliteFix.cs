using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class MakeUserPlanNullable_SqliteFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Precaución: desactivar temporalmente comprobaciones FK para poder recrear la tabla Users en SQLite
            migrationBuilder.Sql("PRAGMA foreign_keys=OFF;");

            // 1) Crear tabla temporal con PlanId NULLABLE (ajusta columnas si tu modelo tiene otras adicionales)
            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS Users_new (
    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Apellido TEXT NOT NULL,
    Email TEXT NOT NULL,
    Telefono TEXT NOT NULL,
    Contraseña TEXT NOT NULL,
    RoleId INTEGER NOT NULL,
    PlanId INTEGER NULL
);
");

            // 2) Copiar los datos desde la tabla actual
            migrationBuilder.Sql(@"
INSERT INTO Users_new (Id, Nombre, Apellido, Email, Telefono, Contraseña, RoleId, PlanId)
SELECT Id, Nombre, Apellido, Email, Telefono, Contraseña, RoleId, PlanId
FROM Users;
");

            // 3) Eliminar la tabla antigua y renombrar la nueva
            migrationBuilder.Sql(@"
DROP TABLE Users;
ALTER TABLE Users_new RENAME TO Users;
");

            // 4) Recrear índices que EF espera
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS IX_Users_PlanId ON Users(PlanId);");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS IX_Users_RoleId ON Users(RoleId);");

            // 5) Restaurar comprobaciones FK
            migrationBuilder.Sql("PRAGMA foreign_keys=ON;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir: crear tabla con PlanId NOT NULL (forzando valor para filas nulas si existen)
            migrationBuilder.Sql("PRAGMA foreign_keys=OFF;");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS Users_old (
    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Apellido TEXT NOT NULL,
    Email TEXT NOT NULL,
    Telefono TEXT NOT NULL,
    Contraseña TEXT NOT NULL,
    RoleId INTEGER NOT NULL,
    PlanId INTEGER NOT NULL
);
");

            migrationBuilder.Sql(@"
INSERT INTO Users_old (Id, Nombre, Apellido, Email, Telefono, Contraseña, RoleId, PlanId)
SELECT Id, Nombre, Apellido, Email, Telefono, Contraseña, RoleId, COALESCE(PlanId, 0)
FROM Users;
");

            migrationBuilder.Sql(@"
DROP TABLE Users;
ALTER TABLE Users_old RENAME TO Users;
");

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS IX_Users_PlanId ON Users(PlanId);");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS IX_Users_RoleId ON Users(RoleId);");

            migrationBuilder.Sql("PRAGMA foreign_keys=ON;");
        }
    }
}