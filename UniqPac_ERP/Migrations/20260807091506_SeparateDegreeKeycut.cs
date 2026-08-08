using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class SeparateDegreeKeycut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DegreeKeycut",
                table: "CylinderMasters",
                newName: "Keycut");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "CylinderMasters",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "CylinderMasters");

            migrationBuilder.RenameColumn(
                name: "Keycut",
                table: "CylinderMasters",
                newName: "DegreeKeycut");
        }
    }
}
