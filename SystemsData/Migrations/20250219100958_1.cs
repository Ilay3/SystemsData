using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SystemsData.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "SystemEntity",
                columns: table => new
                {
                    SystemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemEntity", x => x.SystemId);
                });

            migrationBuilder.CreateTable(
                name: "Base_Components",
                columns: table => new
                {
                    ComponentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SystemId = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ReplacementInfo = table.Column<string>(type: "text", nullable: true),
                    FileAttachment = table.Column<byte[]>(type: "bytea", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    SystemEntitySystemId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Components", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Base_Components_SystemEntity_SystemEntitySystemId",
                        column: x => x.SystemEntitySystemId,
                        principalTable: "SystemEntity",
                        principalColumn: "SystemId");
                });

            migrationBuilder.CreateTable(
                name: "Component_Parts",
                columns: table => new
                {
                    PartId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentComponentId = table.Column<int>(type: "integer", nullable: false),
                    PartType = table.Column<string>(type: "text", nullable: false),
                    PartNumber = table.Column<string>(type: "text", nullable: false),
                    PartName = table.Column<string>(type: "text", nullable: false),
                    AdditionalDetails = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component_Parts", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_Component_Parts_Base_Components_ParentComponentId",
                        column: x => x.ParentComponentId,
                        principalTable: "Base_Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Base_Components_SystemEntitySystemId",
                table: "Base_Components",
                column: "SystemEntitySystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Component_Parts_ParentComponentId",
                table: "Component_Parts",
                column: "ParentComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Component_Parts");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Base_Components");

            migrationBuilder.DropTable(
                name: "SystemEntity");
        }
    }
}
