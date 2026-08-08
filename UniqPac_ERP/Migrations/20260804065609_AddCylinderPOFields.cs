using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddCylinderPOFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BoreId",
                table: "PurchaseOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoilSize",
                table: "PurchaseOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POType",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RepeatSize",
                table: "PurchaseOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CylinderSize",
                table: "PurchaseOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobSize",
                table: "PurchaseOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCylinders",
                table: "PurchaseOrderItems",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoreId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CoilSize",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "POType",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RepeatSize",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CylinderSize",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "JobSize",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "NumberOfCylinders",
                table: "PurchaseOrderItems");
        }
    }
}
