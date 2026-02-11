using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlanFromUserAddingSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Plans_PlanId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PlanId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Fecha", "MetodoPago", "Pagado", "SubscriptionId" },
                values: new object[] { new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 1 });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Fecha", "MetodoPago", "Pagado", "SubscriptionId" },
                values: new object[] { new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, 1 });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Fecha", "MetodoPago", "SubscriptionId" },
                values: new object[] { new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2 });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "EndDate", "IsActive", "PlanId", "StartDate", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAECV99eopkaXJB7mylI9M5lOVR3X0ugfP/ogduxPJJMFkQUJE8pYczgQmF4ER4oGTHA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAEFMPITMIAhLP3INx8RKIDR9ptVPTFqYCt3P+A2mb3MiWi0RoWovzjrZjDWzoUVXycA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Contraseña",
                value: "AQAAAAIAAYagAAAAELt2q2gb7/fHYD3WRrS8/XsvRPu07Vc0Snme+lnpJqw8DWlFBIGPDerx/0uNldPT9w==");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SubscriptionId",
                table: "Payments",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanId",
                table: "Subscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Subscriptions_SubscriptionId",
                table: "Payments",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Subscriptions_SubscriptionId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SubscriptionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Fecha",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Fecha", "Pagado" },
                values: new object[] { "2025-10-10", true });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Fecha", "Pagado" },
                values: new object[] { "2025-11-10", false });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: "2025-10-10");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Contraseña", "PlanId" },
                values: new object[] { "AQAAAAIAAYagAAAAEH13Z14IdpKTs0lAciPjEH68V5utaF7JlEddnng8vsh/GD081noBroI0gfAptt7eLA==", 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Contraseña", "PlanId" },
                values: new object[] { "AQAAAAIAAYagAAAAEJi7bz6YE8D46GMlho74KYtok+lpjm40WtguxRQF3+S0AMiJ4hwU7SHNnGeYdaRjvA==", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Contraseña", "PlanId" },
                values: new object[] { "AQAAAAIAAYagAAAAEEV9lsMZxMBxWh4R6JMmYv6py8+bQhINumKqoKPJ4qeKLS+U6gvsNwZ5HahPuqa98Q==", null });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PlanId",
                table: "Users",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Plans_PlanId",
                table: "Users",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
