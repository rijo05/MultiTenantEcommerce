using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantEcommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TenantChangeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Tenants",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tenants",
                newName: "CompanyName");
        }
    }
}
