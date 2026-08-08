using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqPac_ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddCylindersMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CylinderMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerJobId = table.Column<int>(type: "int", nullable: false),
                    CylinderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CylinderCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NoOfCylinders = table.Column<int>(type: "int", nullable: true),
                    CylinderSize = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CoilSize = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RepeatSize = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PetSize = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Structure = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BoreId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DegreeKeycut = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductPacked = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CylinderMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CylinderMasters_CustomerJobs_CustomerJobId",
                        column: x => x.CustomerJobId,
                        principalTable: "CustomerJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CylinderMasters_CustomerJobId",
                table: "CylinderMasters",
                column: "CustomerJobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CylinderMasters");
        }
    }
}
