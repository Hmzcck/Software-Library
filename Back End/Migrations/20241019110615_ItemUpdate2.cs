using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Back_End.Migrations
{
    /// <inheritdoc />
    public partial class ItemUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01500b4e-5ec6-4dbe-8270-b5dee8c80ce3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62f2ed5b-cdbf-49c2-9d1d-ded9e7451e79");

            migrationBuilder.AddColumn<string>(
                name: "Repository",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "74b078cc-7c27-429a-9e54-67cac4d61225", null, "Admin", "ADMIN" },
                    { "a24710c5-369e-46ab-9815-bd6f80f86397", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74b078cc-7c27-429a-9e54-67cac4d61225");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a24710c5-369e-46ab-9815-bd6f80f86397");

            migrationBuilder.DropColumn(
                name: "Repository",
                table: "Items");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01500b4e-5ec6-4dbe-8270-b5dee8c80ce3", null, "User", "USER" },
                    { "62f2ed5b-cdbf-49c2-9d1d-ded9e7451e79", null, "Admin", "ADMIN" }
                });
        }
    }
}
