using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChargeStation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargeStation_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Connector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ChargeStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaxCurrent = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connector", x => new { x.Id, x.ChargeStationId });
                    table.ForeignKey(
                        name: "FK_Connector_ChargeStation_ChargeStationId",
                        column: x => x.ChargeStationId,
                        principalTable: "ChargeStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargeStation_GroupId",
                table: "ChargeStation",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Connector_ChargeStationId",
                table: "Connector",
                column: "ChargeStationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connector");

            migrationBuilder.DropTable(
                name: "ChargeStation");

            migrationBuilder.DropTable(
                name: "Group");
        }
    }
}
