using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GeneralTemplate.Migrations
{
    public partial class holymigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 20, nullable: false),
                    LastName = table.Column<string>(maxLength: 40, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "DbGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupName = table.Column<string>(nullable: false),
                    GroupMemberOne = table.Column<string>(nullable: false),
                    GroupMemberTwo = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbGroups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_DbGroups_DbUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DbUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommentText = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_DbComments_DbGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DbGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbComments_DbUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DbUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbRSVPs",
                columns: table => new
                {
                    RSVPId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbRSVPs", x => x.RSVPId);
                    table.ForeignKey(
                        name: "FK_DbRSVPs_DbGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DbGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbRSVPs_DbUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DbUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbComments_GroupId",
                table: "DbComments",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DbComments_UserId",
                table: "DbComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DbGroups_UserId",
                table: "DbGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DbRSVPs_GroupId",
                table: "DbRSVPs",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DbRSVPs_UserId",
                table: "DbRSVPs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbComments");

            migrationBuilder.DropTable(
                name: "DbRSVPs");

            migrationBuilder.DropTable(
                name: "DbGroups");

            migrationBuilder.DropTable(
                name: "DbUsers");
        }
    }
}
