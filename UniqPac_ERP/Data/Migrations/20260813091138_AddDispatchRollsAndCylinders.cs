using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDispatchRollsAndCylinders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceiptNoteCylinders_Items_ItemId",
                table: "GoodsReceiptNoteCylinders");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "GoodsReceiptNoteCylinders",
                newName: "CylinderMasterId");

            migrationBuilder.RenameIndex(
                name: "IX_GoodsReceiptNoteCylinders_ItemId",
                table: "GoodsReceiptNoteCylinders",
                newName: "IX_GoodsReceiptNoteCylinders_CylinderMasterId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDispatched",
                table: "GoodsReceiptNoteRolls",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "GoodsReceiptNoteCylinders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DispatchItemCylinders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DispatchItemId = table.Column<int>(type: "int", nullable: false),
                    GoodsReceiptNoteCylinderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchItemCylinders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatchItemCylinders_DispatchItems_DispatchItemId",
                        column: x => x.DispatchItemId,
                        principalTable: "DispatchItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispatchItemCylinders_GoodsReceiptNoteCylinders_GoodsReceiptNoteCylinderId",
                        column: x => x.GoodsReceiptNoteCylinderId,
                        principalTable: "GoodsReceiptNoteCylinders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DispatchItemRolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DispatchItemId = table.Column<int>(type: "int", nullable: false),
                    GoodsReceiptNoteRollId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchItemRolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatchItemRolls_DispatchItems_DispatchItemId",
                        column: x => x.DispatchItemId,
                        principalTable: "DispatchItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispatchItemRolls_GoodsReceiptNoteRolls_GoodsReceiptNoteRollId",
                        column: x => x.GoodsReceiptNoteRollId,
                        principalTable: "GoodsReceiptNoteRolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispatchItemCylinders_DispatchItemId",
                table: "DispatchItemCylinders",
                column: "DispatchItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchItemCylinders_GoodsReceiptNoteCylinderId",
                table: "DispatchItemCylinders",
                column: "GoodsReceiptNoteCylinderId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchItemRolls_DispatchItemId",
                table: "DispatchItemRolls",
                column: "DispatchItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchItemRolls_GoodsReceiptNoteRollId",
                table: "DispatchItemRolls",
                column: "GoodsReceiptNoteRollId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceiptNoteCylinders_CylinderMasters_CylinderMasterId",
                table: "GoodsReceiptNoteCylinders",
                column: "CylinderMasterId",
                principalTable: "CylinderMasters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceiptNoteCylinders_CylinderMasters_CylinderMasterId",
                table: "GoodsReceiptNoteCylinders");

            migrationBuilder.DropTable(
                name: "DispatchItemCylinders");

            migrationBuilder.DropTable(
                name: "DispatchItemRolls");

            migrationBuilder.DropColumn(
                name: "IsDispatched",
                table: "GoodsReceiptNoteRolls");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GoodsReceiptNoteCylinders");

            migrationBuilder.RenameColumn(
                name: "CylinderMasterId",
                table: "GoodsReceiptNoteCylinders",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_GoodsReceiptNoteCylinders_CylinderMasterId",
                table: "GoodsReceiptNoteCylinders",
                newName: "IX_GoodsReceiptNoteCylinders_ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceiptNoteCylinders_Items_ItemId",
                table: "GoodsReceiptNoteCylinders",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
