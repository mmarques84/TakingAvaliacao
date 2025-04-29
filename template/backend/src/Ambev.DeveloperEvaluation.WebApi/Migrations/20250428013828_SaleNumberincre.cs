using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class SaleNumberincre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "SalesItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<long>(
                name: "SaleNumber",
                table: "Sales",
                type: "bigint",
                nullable: false,
                defaultValueSql: "nextval('public.\"SaleNumberSequence\"')",
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "SalesItems");

            migrationBuilder.AlterColumn<long>(
                name: "SaleNumber",
                table: "Sales",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValueSql: "nextval('public.\"SaleNumberSequence\"')");
        }
    }
}
