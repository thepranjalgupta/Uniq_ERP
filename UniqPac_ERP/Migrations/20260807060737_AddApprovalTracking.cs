using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovalTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdminApprovalDate",
                table: "SalesOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "SalesOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByAdminId",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByManagerId",
                table: "SalesOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerApprovalDate",
                table: "SalesOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AdminApprovalDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "PurchaseOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByAdminId",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByManagerId",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerApprovalDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AdminApprovalDate",
                table: "GoodsReceiptNotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "GoodsReceiptNotes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByAdminId",
                table: "GoodsReceiptNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByManagerId",
                table: "GoodsReceiptNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerApprovalDate",
                table: "GoodsReceiptNotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApprovalHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_AspNetUsers_ActionById",
                        column: x => x.ActionById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_ActionById",
                table: "ApprovalHistories",
                column: "ActionById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalHistories");

            migrationBuilder.DropColumn(
                name: "AdminApprovalDate",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ApprovedByAdminId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ApprovedByManagerId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "ManagerApprovalDate",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "AdminApprovalDate",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ApprovedByAdminId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ApprovedByManagerId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ManagerApprovalDate",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "AdminApprovalDate",
                table: "GoodsReceiptNotes");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "GoodsReceiptNotes");

            migrationBuilder.DropColumn(
                name: "ApprovedByAdminId",
                table: "GoodsReceiptNotes");

            migrationBuilder.DropColumn(
                name: "ApprovedByManagerId",
                table: "GoodsReceiptNotes");

            migrationBuilder.DropColumn(
                name: "ManagerApprovalDate",
                table: "GoodsReceiptNotes");
        }
    }
}
