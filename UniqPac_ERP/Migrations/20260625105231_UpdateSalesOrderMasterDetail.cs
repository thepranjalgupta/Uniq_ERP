using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalesOrderMasterDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_CustomerJobs_CustomerJobId",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_Items_ItemId",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_CustomerJobId",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_ItemId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "CustomerJobId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "SalesOrders");

            migrationBuilder.CreateTable(
                name: "SalesOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesOrderId = table.Column<int>(type: "int", nullable: false),
                    CustomerJobId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderItems_CustomerJobs_CustomerJobId",
                        column: x => x.CustomerJobId,
                        principalTable: "CustomerJobs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrderItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrderItems_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItems_CustomerJobId",
                table: "SalesOrderItems",
                column: "CustomerJobId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItems_ItemId",
                table: "SalesOrderItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItems_SalesOrderId",
                table: "SalesOrderItems",
                column: "SalesOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesOrderItems");

            migrationBuilder.AddColumn<int>(
                name: "CustomerJobId",
                table: "SalesOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "SalesOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "SalesOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "SalesOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CustomerJobId",
                table: "SalesOrders",
                column: "CustomerJobId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_ItemId",
                table: "SalesOrders",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_CustomerJobs_CustomerJobId",
                table: "SalesOrders",
                column: "CustomerJobId",
                principalTable: "CustomerJobs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_Items_ItemId",
                table: "SalesOrders",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
