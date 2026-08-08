using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class MovePackingTypeToSOItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackingType",
                table: "SalesOrders");

            migrationBuilder.AddColumn<string>(
                name: "PackingType",
                table: "SalesOrderItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackingType",
                table: "SalesOrderItems");

            migrationBuilder.AddColumn<string>(
                name: "PackingType",
                table: "SalesOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
