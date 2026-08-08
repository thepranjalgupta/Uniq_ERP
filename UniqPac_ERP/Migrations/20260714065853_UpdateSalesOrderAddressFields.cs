using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalesOrderAddressFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConsigneeNameAndAddress",
                table: "SalesOrders",
                newName: "ShippingAddress");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "SalesOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingName",
                table: "SalesOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsShippingSameAsBilling",
                table: "SalesOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShippingName",
                table: "SalesOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "BillingName",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "IsShippingSameAsBilling",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ShippingName",
                table: "SalesOrders");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress",
                table: "SalesOrders",
                newName: "ConsigneeNameAndAddress");
        }
    }
}
