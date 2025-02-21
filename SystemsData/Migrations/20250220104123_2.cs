using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemsData.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Base_Components_SystemEntity_SystemEntitySystemId",
                table: "Base_Components");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemEntity",
                table: "SystemEntity");

            migrationBuilder.RenameTable(
                name: "SystemEntity",
                newName: "SystemEntities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemEntities",
                table: "SystemEntities",
                column: "SystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Base_Components_SystemEntities_SystemEntitySystemId",
                table: "Base_Components",
                column: "SystemEntitySystemId",
                principalTable: "SystemEntities",
                principalColumn: "SystemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Base_Components_SystemEntities_SystemEntitySystemId",
                table: "Base_Components");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemEntities",
                table: "SystemEntities");

            migrationBuilder.RenameTable(
                name: "SystemEntities",
                newName: "SystemEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemEntity",
                table: "SystemEntity",
                column: "SystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Base_Components_SystemEntity_SystemEntitySystemId",
                table: "Base_Components",
                column: "SystemEntitySystemId",
                principalTable: "SystemEntity",
                principalColumn: "SystemId");
        }
    }
}
