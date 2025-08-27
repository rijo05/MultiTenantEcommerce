using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantEcommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRowVersionToStockForRealNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Stocks",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Stocks");
        }
    }
}
