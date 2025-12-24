using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourseProjectYacenko.Migrations
{
    /// <inheritdoc />
    public partial class firatMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PassportData = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MonthlyFee = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    InternetTrafficGB = table.Column<int>(type: "int", nullable: false),
                    MinutesCount = table.Column<int>(type: "int", nullable: false),
                    SmsCount = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tariffs_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    BillingPeriod = table.Column<int>(type: "int", nullable: false),
                    TariffId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Tariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "BillingPeriod", "Cost", "Description", "Name", "TariffId", "Type" },
                values: new object[,]
                {
                    { 1, 2, 50.00m, "Безлимитный YouTube", "YouTube безлимит", null, 3 },
                    { 2, 2, 100.00m, "Защита устройства", "Антивирус", null, 4 },
                    { 3, 2, 30.00m, "Безлимитная музыка", "Музыка", null, 3 },
                    { 4, 2, 80.00m, "Звонки за границу", "Международные звонки", null, 1 }
                });

            migrationBuilder.InsertData(
                table: "Tariffs",
                columns: new[] { "Id", "AppUserId", "Description", "InternetTrafficGB", "MinutesCount", "MonthlyFee", "Name", "SmsCount" },
                values: new object[,]
                {
                    { 1, null, "Для новых клиентов", 5, 100, 300.00m, "Базовый", 50 },
                    { 2, null, "Популярный тариф", 15, 300, 500.00m, "Стандарт", 100 },
                    { 3, null, "Для активных пользователей", 30, 1000, 1000.00m, "Премиум", 500 },
                    { 4, null, "Минимальный пакет", 2, 50, 200.00m, "Эконом", 20 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Balance", "Email", "FullName", "IsActive", "LastLoginDate", "PassportData", "PasswordHash", "PhoneNumber", "RefreshToken", "RefreshTokenExpiryTime", "RegistrationDate", "Role" },
                values: new object[,]
                {
                    { 1, "г. Москва, ул. Административная, д. 1", 10000.00m, "admin@mobileoperator.ru", "Администратор Системы", true, null, "0000 000000", "$2a$11$XMcVtQQ3bQwNX2lozQfoyOyreHMPifSuS/.sOxqpUzm6co9q69vQ6", "9998887766", null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin" },
                    { 2, "г. Москва, ул. Примерная, д. 10", 1500.50m, "ivanov@example.com", "Иванов Иван Иванович", true, null, "1234 567890", "$2a$11$PTdbwEfGpJOE5dTidkUeUOljftB1L2p1E1voVh5DkSco9VNpOiw7O", "9991112233", null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "User" },
                    { 3, "г. Санкт-Петербург, ул. Тестовая, д. 5", 750.25m, "petrova@example.com", "Петрова Анна Сергеевна", true, null, "5678 901234", "$2a$11$QWM.SQuD6Jb1GZhF6g55bO9UyzKrAtbCAe6oqVCvfmr/LR9rJxfuC", "9992223344", null, null, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "User" }
                });

            migrationBuilder.InsertData(
                table: "Applications",
                columns: new[] { "Id", "AppUserId", "Comment", "CreationDate", "ProcessingDate", "Status", "Type" },
                values: new object[,]
                {
                    { 1, 2, "Новое подключение обработано", new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 2, 14, 30, 0, 0, DateTimeKind.Utc), 2, 0 },
                    { 2, 3, "Запрос на смену тарифа", new DateTime(2024, 3, 5, 11, 20, 0, 0, DateTimeKind.Utc), null, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "AppUserId", "PaymentDateTime", "PaymentMethod", "Status" },
                values: new object[,]
                {
                    { 1, 1000.00m, 2, new DateTime(2024, 2, 5, 10, 30, 0, 0, DateTimeKind.Utc), 0, 1 },
                    { 2, 500.50m, 2, new DateTime(2024, 3, 10, 14, 15, 0, 0, DateTimeKind.Utc), 3, 1 },
                    { 3, 1000.00m, 3, new DateTime(2024, 3, 15, 9, 45, 0, 0, DateTimeKind.Utc), 1, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AppUserId",
                table: "Applications",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AppUserId",
                table: "Payments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_TariffId",
                table: "Services",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_Tariffs_AppUserId",
                table: "Tariffs",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Tariffs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
