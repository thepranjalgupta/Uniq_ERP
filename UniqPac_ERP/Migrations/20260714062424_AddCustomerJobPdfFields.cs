using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerJobPdfFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CylinderCharges",
                table: "CustomerJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CylinderStatus",
                table: "CustomerJobs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direction",
                table: "CustomerJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobCode",
                table: "CustomerJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobSize",
                table: "CustomerJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LinkedItemId",
                table: "CustomerJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RollWeight",
                table: "CustomerJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SampleRequired",
                table: "CustomerJobs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShadeMatch",
                table: "CustomerJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specs",
                table: "CustomerJobs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerJobs_LinkedItemId",
                table: "CustomerJobs",
                column: "LinkedItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerJobs_Items_LinkedItemId",
                table: "CustomerJobs",
                column: "LinkedItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerJobs_Items_LinkedItemId",
                table: "CustomerJobs");

            migrationBuilder.DropIndex(
                name: "IX_CustomerJobs_LinkedItemId",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "CylinderCharges",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "CylinderStatus",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "JobCode",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "JobSize",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "LinkedItemId",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "RollWeight",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "SampleRequired",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "ShadeMatch",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "Specs",
                table: "CustomerJobs");
        }
    }
}
