using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PredictaPay.Migrations
{
    /// <inheritdoc />
    public partial class InitialIntIdsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    BillName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    DueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    RecurrenceType = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    ReminderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BillId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReminderType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSent = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.ReminderId);
                    table.ForeignKey(
                        name: "FK_Reminders_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 19, 3, 47, 42, 860, DateTimeKind.Utc).AddTicks(6430), "john@example.com", "John", "Doe", "AQAAAAIAAYagAAAAENptXfHm/1aTmuwLuGUoG78eSrRuQLrOfsXKEk1iMO8UXwG0mBLgtUEpid9cz5O5Qw==", "User" },
                    { 2, new DateTime(2026, 3, 29, 3, 47, 42, 966, DateTimeKind.Utc).AddTicks(470), "jane@example.com", "Jane", "Smith", "AQAAAAIAAYagAAAAEKHbVV6MCS3u4bp103KbyBB7uYL0RNkbUY8RA1x/StTJQtXuzNT2oZ2fQZ369trCNA==", "User" }
                });

            migrationBuilder.InsertData(
                table: "Bills",
                columns: new[] { "BillId", "Amount", "BillName", "Category", "CreatedAt", "DueDate", "Notes", "RecurrenceType", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 125.50m, "Electricity Bill", "Utilities", new DateTime(2026, 4, 8, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(330), new DateOnly(2026, 4, 22), "Monthly electric bill", 2, 0, 1 },
                    { 2, 79.99m, "Internet Bill", "Utilities", new DateTime(2026, 4, 3, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(1010), new DateOnly(2026, 4, 20), "Monthly internet service", 2, 0, 1 },
                    { 3, 150.00m, "Car Insurance", "Insurance", new DateTime(2026, 4, 13, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(1030), new DateOnly(2026, 4, 27), "Auto insurance premium", 2, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Reminders",
                columns: new[] { "ReminderId", "BillId", "IsSent", "ReminderDate", "ReminderType" },
                values: new object[,]
                {
                    { 1, 1, false, new DateTime(2026, 4, 20, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(5470), 0 },
                    { 2, 2, false, new DateTime(2026, 4, 19, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(7600), 2 },
                    { 3, 3, true, new DateTime(2026, 4, 25, 3, 47, 42, 983, DateTimeKind.Utc).AddTicks(7610), 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_UserId",
                table: "Bills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_BillId",
                table: "Reminders",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
