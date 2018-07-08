using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HTEC.ChampionsLeague.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AwayTeam = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    HomeTeam = table.Column<string>(nullable: true),
                    KickoffAt = table.Column<DateTime>(nullable: false),
                    LeagueTitle = table.Column<string>(nullable: true),
                    Matchday = table.Column<int>(nullable: false),
                    Score = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Group = table.Column<string>(nullable: true),
                    LeagueTitle = table.Column<string>(nullable: true),
                    Matchday = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Standing",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Draw = table.Column<int>(nullable: false),
                    GoalDifference = table.Column<int>(nullable: false),
                    Goals = table.Column<int>(nullable: false),
                    GoalsAgainst = table.Column<int>(nullable: false),
                    Lose = table.Column<int>(nullable: false),
                    PlayedGames = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    TableId = table.Column<int>(nullable: false),
                    Team = table.Column<string>(nullable: true),
                    Win = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Standing_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Standing_TableId",
                table: "Standing",
                column: "TableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Standing");

            migrationBuilder.DropTable(
                name: "Table");
        }
    }
}
