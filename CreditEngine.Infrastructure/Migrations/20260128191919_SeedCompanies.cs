using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CreditEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "AnnualRevenue", "Cash", "Cnpj", "Ebitda", "TotalDebt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 1800000m, 200000m, "12.345.678/0001-90", 360000m, 500000m },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 600000m, 50000m, "98.765.432/0001-10", 90000m, 150000m },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 3600000m, 400000m, "45.987.321/0001-55", 900000m, 1200000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
