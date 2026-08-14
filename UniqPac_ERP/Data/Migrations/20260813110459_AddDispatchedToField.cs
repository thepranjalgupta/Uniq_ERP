using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDispatchedToField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DispatchedTo",
                table: "GoodsReceiptNoteRolls",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DispatchedTo",
                table: "GoodsReceiptNoteCylinders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DispatchedTo",
                table: "GoodsReceiptNoteRolls");

            migrationBuilder.DropColumn(
                name: "DispatchedTo",
                table: "GoodsReceiptNoteCylinders");
        }
    }
}
