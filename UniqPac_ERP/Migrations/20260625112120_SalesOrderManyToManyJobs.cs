using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class SalesOrderManyToManyJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "CustomerPORef",
                table: "SalesOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuotationRef",
                table: "SalesOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "SalesOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SalesOrderJobLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesOrderId = table.Column<int>(type: "int", nullable: false),
                    CustomerJobId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderJobLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderJobLinks_CustomerJobs_CustomerJobId",
                        column: x => x.CustomerJobId,
                        principalTable: "CustomerJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrderJobLinks_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderJobLinks_CustomerJobId",
                table: "SalesOrderJobLinks",
                column: "CustomerJobId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderJobLinks_SalesOrderId",
                table: "SalesOrderJobLinks",
                column: "SalesOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesOrderJobLinks");

            migrationBuilder.DropColumn(
                name: "CustomerPORef",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "QuotationRef",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "SalesOrders");

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
    }
}
