using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GeneralTemplate.Migrations
{
    public partial class wastedmigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SignedUpUserId",
                table: "DbGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DbSignUpUsers",
                columns: table => new
                {
                    SignUpId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SignedUpHours = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    SignedUpUserId = table.Column<int>(nullable: false),
                    SignUpUserUserId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSignUpUsers", x => x.SignUpId);
                    table.ForeignKey(
                        name: "FK_DbSignUpUsers_DbGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DbGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbSignUpUsers_DbUsers_SignUpUserUserId",
                        column: x => x.SignUpUserUserId,
                        principalTable: "DbUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbSignUpUsers_GroupId",
                table: "DbSignUpUsers",
                column: "GroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DbSignUpUsers_SignUpUserUserId",
                table: "DbSignUpUsers",
                column: "SignUpUserUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbSignUpUsers");

            migrationBuilder.DropColumn(
                name: "SignedUpUserId",
                table: "DbGroups");
        }
    }
}
