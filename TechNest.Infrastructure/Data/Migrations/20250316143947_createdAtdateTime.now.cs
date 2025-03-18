using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechNest.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class createdAtdateTimenow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-47d8-90ab-cdef12345677"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-47d8-90ab-cdef12345678"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[] { new Guid("a1b2c3d4-e5f6-47d8-90ab-cdef12345678"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electronic devices", "Electronics" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "Name", "Price" },
                values: new object[] { new Guid("a1b2c3d4-e5f6-47d8-90ab-cdef12345677"), new Guid("a1b2c3d4-e5f6-47d8-90ab-cdef12345678"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Apple Iphone 12", "Iphone 12", 1000m });
        }
    }
}
