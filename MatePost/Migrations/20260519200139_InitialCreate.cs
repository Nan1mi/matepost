using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace MatePost.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "User"),
                    LoyaltyPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parcels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrackingNumber = table.Column<string>(type: "TEXT", nullable: false),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: false),
                    SenderName = table.Column<string>(type: "TEXT", nullable: false),
                    SenderPhone = table.Column<string>(type: "TEXT", nullable: false),
                    SenderCityId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiverName = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverPhone = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverCityId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentCityId = table.Column<int>(type: "INTEGER", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    WeightKg = table.Column<decimal>(type: "TEXT", precision: 8, scale: 3, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstimatedDelivery = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcels_Cities_CurrentCityId",
                        column: x => x.CurrentCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Parcels_Cities_ReceiverCityId",
                        column: x => x.ReceiverCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parcels_Cities_SenderCityId",
                        column: x => x.SenderCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parcels_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackingEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParcelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CityId = table.Column<int>(type: "INTEGER", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingEvents_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TrackingEvents_Parcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "Parcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Latitude", "Longitude", "Name", "Region" },
                values: new object[,]
                {
                    { 1, 50.450099999999999, 30.523399999999999, "Київ", "Київська" },
                    { 2, 49.993499999999997, 36.230400000000003, "Харків", "Харківська" },
                    { 3, 46.482500000000002, 30.723299999999998, "Одеса", "Одеська" },
                    { 4, 48.464700000000001, 35.046199999999999, "Дніпро", "Дніпропетровська" },
                    { 5, 47.838799999999999, 35.139600000000002, "Запоріжжя", "Запорізька" },
                    { 6, 49.839700000000001, 24.029699999999998, "Львів", "Львівська" },
                    { 7, 47.907800000000002, 33.342799999999997, "Кривий Ріг", "Дніпропетровська" },
                    { 8, 46.975000000000001, 31.994599999999998, "Миколаїв", "Миколаївська" },
                    { 9, 47.095599999999997, 37.549399999999999, "Маріуполь", "Донецька" },
                    { 10, 48.573999999999998, 39.3078, "Луганськ", "Луганська" },
                    { 11, 49.232799999999997, 28.4682, "Вінниця", "Вінницька" },
                    { 12, 46.635399999999997, 32.616900000000001, "Херсон", "Херсонська" },
                    { 13, 49.588299999999997, 34.551400000000001, "Полтава", "Полтавська" },
                    { 14, 51.498199999999997, 31.289300000000001, "Чернігів", "Чернігівська" },
                    { 15, 49.444400000000002, 32.059800000000003, "Черкаси", "Черкаська" },
                    { 16, 50.907699999999998, 34.798099999999998, "Суми", "Сумська" },
                    { 17, 50.2547, 28.6587, "Житомир", "Житомирська" },
                    { 18, 50.619900000000001, 26.2516, "Рівне", "Рівненська" },
                    { 19, 48.922600000000003, 24.711099999999998, "Івано-Франківськ", "Івано-Франківська" },
                    { 20, 49.5535, 25.594799999999999, "Тернопіль", "Тернопільська" },
                    { 21, 50.747199999999999, 25.325399999999998, "Луцьк", "Волинська" },
                    { 22, 48.620800000000003, 22.2879, "Ужгород", "Закарпатська" },
                    { 23, 49.422899999999998, 26.996400000000001, "Хмельницький", "Хмельницька" },
                    { 24, 48.679200000000002, 26.583300000000001, "Кам'янець-Подільський", "Хмельницька" },
                    { 25, 48.513199999999998, 32.259700000000002, "Кропивницький", "Кіровоградська" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_CurrentCityId",
                table: "Parcels",
                column: "CurrentCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_ReceiverCityId",
                table: "Parcels",
                column: "ReceiverCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_SenderCityId",
                table: "Parcels",
                column: "SenderCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcels_SenderId",
                table: "Parcels",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingEvents_CityId",
                table: "TrackingEvents",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingEvents_ParcelId",
                table: "TrackingEvents",
                column: "ParcelId");

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
                name: "TrackingEvents");

            migrationBuilder.DropTable(
                name: "Parcels");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
