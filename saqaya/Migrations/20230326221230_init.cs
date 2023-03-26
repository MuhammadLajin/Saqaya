using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace saqaya.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    marketingConsent = table.Column<bool>(type: "bit", nullable: false),
                    accessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "accessToken", "email", "firstName", "lastName", "marketingConsent" },
                values: new object[] { "testKeyONE", new DateTime(2023, 3, 27, 0, 12, 29, 446, DateTimeKind.Local).AddTicks(9520), false, null, "testUserOne", "User", "One", false });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "accessToken", "email", "firstName", "lastName", "marketingConsent" },
                values: new object[] { "testKeyTWO", new DateTime(2023, 3, 27, 0, 12, 29, 450, DateTimeKind.Local).AddTicks(2416), false, null, "testUserTWO", "User", "TWO", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
