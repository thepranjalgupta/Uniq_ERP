using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentStock",
                table: "Items",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "GoodsReceiptNoteItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StockLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RunningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockLedgers_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNoteItems_ItemId",
                table: "GoodsReceiptNoteItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_ItemId",
                table: "StockLedgers",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceiptNoteItems_Items_ItemId",
                table: "GoodsReceiptNoteItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceiptNoteItems_Items_ItemId",
                table: "GoodsReceiptNoteItems");

            migrationBuilder.DropTable(
                name: "StockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptNoteItems_ItemId",
                table: "GoodsReceiptNoteItems");

            migrationBuilder.DropColumn(
                name: "CurrentStock",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "GoodsReceiptNoteItems");
        }
    }
}
