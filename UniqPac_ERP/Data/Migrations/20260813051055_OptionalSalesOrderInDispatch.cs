using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Data.Migrations
{
    /// <inheritdoc />
    public partial class OptionalSalesOrderInDispatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SalesOrderItemId",
                table: "DispatchItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CylinderMasterId",
                table: "DispatchItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "DispatchItems",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SalesOrderId",
                table: "Dispatches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchItems_CylinderMasterId",
                table: "DispatchItems",
                column: "CylinderMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchItems_ItemId",
                table: "DispatchItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatchItems_CylinderMasters_CylinderMasterId",
                table: "DispatchItems",
                column: "CylinderMasterId",
                principalTable: "CylinderMasters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatchItems_Items_ItemId",
                table: "DispatchItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatchItems_CylinderMasters_CylinderMasterId",
                table: "DispatchItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DispatchItems_Items_ItemId",
                table: "DispatchItems");

            migrationBuilder.DropIndex(
                name: "IX_DispatchItems_CylinderMasterId",
                table: "DispatchItems");

            migrationBuilder.DropIndex(
                name: "IX_DispatchItems_ItemId",
                table: "DispatchItems");

            migrationBuilder.DropColumn(
                name: "CylinderMasterId",
                table: "DispatchItems");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "DispatchItems");

            migrationBuilder.AlterColumn<int>(
                name: "SalesOrderItemId",
                table: "DispatchItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SalesOrderId",
                table: "Dispatches",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
