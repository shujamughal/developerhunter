using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProfile.Migrations
{
    public partial class initialCreate2232 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5e3a7720-4002-460d-b143-7f721b599e9e", "1", "CompanyAdmin", "CompanyAdmin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e3a7720-4002-460d-b143-7f721b599e9e");
        }
    }
}
