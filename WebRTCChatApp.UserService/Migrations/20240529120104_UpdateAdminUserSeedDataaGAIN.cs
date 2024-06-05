using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRTCChatApp.UserService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminUserSeedDataaGAIN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "985bb195-bc47-45fd-92c2-ca6bdc3fe87b");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "01082af7-24af-47bc-9f24-4e93394448d4", 0, "9e4a38b2-bb0c-487b-95c3-64664e11baab", "admin@yahoo.com", true, false, null, "ADMIN@YAHOO.COM", "ADMIN", "AQAAAAIAAYagAAAAEKGxTJVmXwxQiEUgKhifrr4YuV/By4RTgfgbNhTHLb/Mn2lWKpo8e12t+CoqtqFQEQ==", null, false, "cd9555b0-1fae-4464-aba6-6ef205d2c2ef", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01082af7-24af-47bc-9f24-4e93394448d4");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "985bb195-bc47-45fd-92c2-ca6bdc3fe87b", 0, "4d56eb84-0470-4fd0-9166-2d94ed755b82", null, false, false, null, null, null, "AQAAAAIAAYagAAAAEB6U94C3KuUanVJYyElecpffIpPeTrJMK9hG8P9tT8/6qvG7g9NBS+IvT6G4tTkwow==", null, false, "25db6d85-138f-47cd-a19d-12cc3a391f88", false, "admin" });
        }
    }
}
