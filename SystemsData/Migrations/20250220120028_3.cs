using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemsData.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Base_Components_SystemEntities_SystemEntitySystemId",
                table: "Base_Components");

            migrationBuilder.DropIndex(
                name: "IX_Base_Components_SystemEntitySystemId",
                table: "Base_Components");

            migrationBuilder.DropColumn(
                name: "SystemEntitySystemId",
                table: "Base_Components");

            migrationBuilder.CreateIndex(
                name: "IX_Base_Components_SystemId",
                table: "Base_Components",
                column: "SystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Base_Components_SystemEntities_SystemId",
                table: "Base_Components",
                column: "SystemId",
                principalTable: "SystemEntities",
                principalColumn: "SystemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Base_Components_SystemEntities_SystemId",
                table: "Base_Components");

            migrationBuilder.DropIndex(
                name: "IX_Base_Components_SystemId",
                table: "Base_Components");

            migrationBuilder.AddColumn<int>(
                name: "SystemEntitySystemId",
                table: "Base_Components",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Base_Components_SystemEntitySystemId",
                table: "Base_Components",
                column: "SystemEntitySystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Base_Components_SystemEntities_SystemEntitySystemId",
                table: "Base_Components",
                column: "SystemEntitySystemId",
                principalTable: "SystemEntities",
                principalColumn: "SystemId");
        }
    }
}
