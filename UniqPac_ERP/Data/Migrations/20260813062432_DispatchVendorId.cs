using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Data.Migrations
{
    /// <inheritdoc />
    public partial class DispatchVendorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Dispatches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Dispatches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dispatches_VendorId",
                table: "Dispatches",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatches_Vendors_VendorId",
                table: "Dispatches",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispatches_Vendors_VendorId",
                table: "Dispatches");

            migrationBuilder.DropIndex(
                name: "IX_Dispatches_VendorId",
                table: "Dispatches");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Dispatches");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
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
