using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerJobFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "LaminationType",
                table: "CustomerJobs");

            migrationBuilder.RenameColumn(
                name: "PrintingColors",
                table: "CustomerJobs",
                newName: "Finish");

            migrationBuilder.RenameColumn(
                name: "MaterialSpecs",
                table: "CustomerJobs",
                newName: "Substrate");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "CustomerJobs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorCount",
                table: "CustomerJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Length",
                table: "CustomerJobs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Thickness",
                table: "CustomerJobs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "CustomerJobs",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorCount",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "Thickness",
                table: "CustomerJobs");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "CustomerJobs");

            migrationBuilder.RenameColumn(
                name: "Substrate",
                table: "CustomerJobs",
                newName: "MaterialSpecs");

            migrationBuilder.RenameColumn(
                name: "Finish",
                table: "CustomerJobs",
                newName: "PrintingColors");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "CustomerJobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "CustomerJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LaminationType",
                table: "CustomerJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
