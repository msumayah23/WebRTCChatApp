using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRTCChatApp.UserService.Migrations
{
    /// <inheritdoc />
    public partial class initialchanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "84cd6639-6232-4111-b627-04ab3b61bb20");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e9367015-452b-4bcd-8757-cba33cce67d8", 0, "a12c4eef-33e6-4e2b-be2e-3f5fb691ef1f", null, false, false, null, null, null, "AQAAAAIAAYagAAAAEPtHSyOaK7c61Is71iZGZDivT8PjQLv5Ph/vZ1QDVOAwjF9hLhfLEti7Rm7uwDnYCw==", null, false, "89fa1677-3138-48bc-a203-1ddcc2960a3e", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e9367015-452b-4bcd-8757-cba33cce67d8");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "84cd6639-6232-4111-b627-04ab3b61bb20", 0, "27c837fb-b9af-4da2-99c6-b1466c4a61ba", "IdentityUser", null, false, false, null, null, null, "@dmin1", null, false, "fb2bce1f-bdbd-4b5a-9c73-6fe5df775c8c", false, "admin" });
        }
    }
}
