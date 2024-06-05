using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRTCChatApp.UserService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminUserSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "201edb6b-9caa-46a5-877f-a576c60590d8");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "985bb195-bc47-45fd-92c2-ca6bdc3fe87b", 0, "4d56eb84-0470-4fd0-9166-2d94ed755b82", null, false, false, null, null, null, "AQAAAAIAAYagAAAAEB6U94C3KuUanVJYyElecpffIpPeTrJMK9hG8P9tT8/6qvG7g9NBS+IvT6G4tTkwow==", null, false, "25db6d85-138f-47cd-a19d-12cc3a391f88", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "985bb195-bc47-45fd-92c2-ca6bdc3fe87b");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "201edb6b-9caa-46a5-877f-a576c60590d8", 0, "d7adb491-968a-4883-9f05-7604ced6e090", null, false, false, null, null, null, "AQAAAAIAAYagAAAAEKNqGTrA46+BP91a+ErgTyd2bUkedKT/6VAsAyDXnY8UBQicikANZkcwhCgF5lEX1w==", null, false, "cbd8ee68-fee2-49db-94f3-37da3457617a", false, "admin" });
        }
    }
}
