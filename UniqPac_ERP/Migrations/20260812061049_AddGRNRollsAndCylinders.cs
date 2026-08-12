using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddGRNRollsAndCylinders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodsReceiptNoteCylinders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsReceiptNoteItemId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    CylinderNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceiptNoteCylinders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptNoteCylinders_GoodsReceiptNoteItems_GoodsReceiptNoteItemId",
                        column: x => x.GoodsReceiptNoteItemId,
                        principalTable: "GoodsReceiptNoteItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptNoteCylinders_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceiptNoteRolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsReceiptNoteItemId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    RollNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RollWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceiptNoteRolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptNoteRolls_GoodsReceiptNoteItems_GoodsReceiptNoteItemId",
                        column: x => x.GoodsReceiptNoteItemId,
                        principalTable: "GoodsReceiptNoteItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceiptNoteRolls_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNoteCylinders_GoodsReceiptNoteItemId",
                table: "GoodsReceiptNoteCylinders",
                column: "GoodsReceiptNoteItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNoteCylinders_ItemId",
                table: "GoodsReceiptNoteCylinders",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNoteRolls_GoodsReceiptNoteItemId",
                table: "GoodsReceiptNoteRolls",
                column: "GoodsReceiptNoteItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptNoteRolls_ItemId",
                table: "GoodsReceiptNoteRolls",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodsReceiptNoteCylinders");

            migrationBuilder.DropTable(
                name: "GoodsReceiptNoteRolls");
        }
    }
}
