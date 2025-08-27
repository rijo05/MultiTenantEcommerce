using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantEcommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixOrdeItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderTenantId_OrderId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderTenantId_OrderId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderTenantId",
                table: "OrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderTenantId",
                table: "OrderItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderTenantId_OrderId",
                table: "OrderItems",
                columns: new[] { "OrderTenantId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderTenantId_OrderId",
                table: "OrderItems",
                columns: new[] { "OrderTenantId", "OrderId" },
                principalTable: "Orders",
                principalColumns: new[] { "TenantId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
