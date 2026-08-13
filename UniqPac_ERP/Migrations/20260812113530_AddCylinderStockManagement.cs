using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddCylinderStockManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CylinderMasterId",
                table: "GoodsReceiptNoteItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentStock",
                table: "CylinderMasters",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CylinderStockLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CylinderMasterId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RunningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CylinderStockLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CylinderStockLedgers_CylinderMasters_CylinderMasterId",
                        column: x => x.CylinderMasterId,
                        principalTable: "CylinderMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNoteItems_CylinderMasterId",
                table: "GoodsReceiptNoteItems",
                column: "CylinderMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CylinderStockLedgers_CylinderMasterId",
                table: "CylinderStockLedgers",
                column: "CylinderMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceiptNoteItems_CylinderMasters_CylinderMasterId",
                table: "GoodsReceiptNoteItems",
                column: "CylinderMasterId",
                principalTable: "CylinderMasters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceiptNoteItems_CylinderMasters_CylinderMasterId",
                table: "GoodsReceiptNoteItems");

            migrationBuilder.DropTable(
                name: "CylinderStockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptNoteItems_CylinderMasterId",
                table: "GoodsReceiptNoteItems");

            migrationBuilder.DropColumn(
                name: "CylinderMasterId",
                table: "GoodsReceiptNoteItems");

            migrationBuilder.DropColumn(
                name: "CurrentStock",
                table: "CylinderMasters");
        }
    }
}
