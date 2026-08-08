using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class MoveJobToSalesOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderItems_CustomerJobs_CustomerJobId",
                table: "SalesOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrderItems_CustomerJobId",
                table: "SalesOrderItems");

            migrationBuilder.DropColumn(
                name: "CustomerJobId",
                table: "SalesOrderItems");

            migrationBuilder.AddColumn<int>(
                name: "CustomerJobId",
                table: "SalesOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CustomerJobId",
                table: "SalesOrders",
                column: "CustomerJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_CustomerJobs_CustomerJobId",
                table: "SalesOrders",
                column: "CustomerJobId",
                principalTable: "CustomerJobs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_CustomerJobs_CustomerJobId",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_CustomerJobId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "CustomerJobId",
                table: "SalesOrders");

            migrationBuilder.AddColumn<int>(
                name: "CustomerJobId",
                table: "SalesOrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItems_CustomerJobId",
                table: "SalesOrderItems",
                column: "CustomerJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderItems_CustomerJobs_CustomerJobId",
                table: "SalesOrderItems",
                column: "CustomerJobId",
                principalTable: "CustomerJobs",
                principalColumn: "Id");
        }
    }
}
