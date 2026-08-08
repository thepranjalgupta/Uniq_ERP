using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesOrderPdfFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConsigneeNameAndAddress",
                table: "SalesOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryTerms",
                table: "SalesOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MktPerson",
                table: "SalesOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeOfTransport",
                table: "SalesOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "SalesOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackingCharges",
                table: "SalesOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackingType",
                table: "SalesOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTerms",
                table: "SalesOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CylinderCharges",
                table: "SalesOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CylinderStatus",
                table: "SalesOrderItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelDate",
                table: "SalesOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direction",
                table: "SalesOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobCode",
                table: "SalesOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobName",
                table: "SalesOrderItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobSize",
                table: "SalesOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RollWeight",
                table: "SalesOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SampleRequired",
                table: "SalesOrderItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShadeMatch",
                table: "SalesOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specs",
                table: "SalesOrderItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsigneeNameAndAddress",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryTerms",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "MktPerson",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ModeOfTransport",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "PackingCharges",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "PackingType",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "PaymentTerms",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "CylinderCharges",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "CylinderStatus",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "DelDate",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "JobCode",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "JobName",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "JobSize",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "RollWeight",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "SampleRequired",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "ShadeMatch",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "Specs",
                table: "SalesOrderItems");
        }
    }
}
