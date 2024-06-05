using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRTCChatApp.UserService.Migrations
{
    /// <inheritdoc />
    public partial class initialchangeddb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e9367015-452b-4bcd-8757-cba33cce67d8");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "201edb6b-9caa-46a5-877f-a576c60590d8", 0, "d7adb491-968a-4883-9f05-7604ced6e090", null, false, false, null, null, null, "AQAAAAIAAYagAAAAEKNqGTrA46+BP91a+ErgTyd2bUkedKT/6VAsAyDXnY8UBQicikANZkcwhCgF5lEX1w==", null, false, "cbd8ee68-fee2-49db-94f3-37da3457617a", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "201edb6b-9caa-46a5-877f-a576c60590d8");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e9367015-452b-4bcd-8757-cba33cce67d8", 0, "a12c4eef-33e6-4e2b-be2e-3f5fb691ef1f", null, false, false, null, null, null, "AQAAAAIAAYagAAAAEPtHSyOaK7c61Is71iZGZDivT8PjQLv5Ph/vZ1QDVOAwjF9hLhfLEti7Rm7uwDnYCw==", null, false, "89fa1677-3138-48bc-a203-1ddcc2960a3e", false, "admin" });
        }
    }
}
