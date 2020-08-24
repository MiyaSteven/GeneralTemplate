using Microsoft.EntityFrameworkCore.Migrations;

namespace GeneralTemplate.Migrations
{
    public partial class oopsmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbSignUpUsers_DbUsers_SignUpUserUserId",
                table: "DbSignUpUsers");

            migrationBuilder.DropIndex(
                name: "IX_DbSignUpUsers_SignUpUserUserId",
                table: "DbSignUpUsers");

            migrationBuilder.DropColumn(
                name: "SignUpUserUserId",
                table: "DbSignUpUsers");

            migrationBuilder.DropColumn(
                name: "SignedUpHours",
                table: "DbSignUpUsers");

            migrationBuilder.DropColumn(
                name: "SignedUpUserId",
                table: "DbSignUpUsers");

            migrationBuilder.AddColumn<string>(
                name: "SignUpTimeLength",
                table: "DbSignUpUsers",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "DbSignUpUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "DbGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DbSignUpUsers_UserId",
                table: "DbSignUpUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbSignUpUsers_DbUsers_UserId",
                table: "DbSignUpUsers",
                column: "UserId",
                principalTable: "DbUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbSignUpUsers_DbUsers_UserId",
                table: "DbSignUpUsers");

            migrationBuilder.DropIndex(
                name: "IX_DbSignUpUsers_UserId",
                table: "DbSignUpUsers");

            migrationBuilder.DropColumn(
                name: "SignUpTimeLength",
                table: "DbSignUpUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DbSignUpUsers");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "DbGroups");

            migrationBuilder.AddColumn<int>(
                name: "SignUpUserUserId",
                table: "DbSignUpUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SignedUpHours",
                table: "DbSignUpUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SignedUpUserId",
                table: "DbSignUpUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DbSignUpUsers_SignUpUserUserId",
                table: "DbSignUpUsers",
                column: "SignUpUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbSignUpUsers_DbUsers_SignUpUserUserId",
                table: "DbSignUpUsers",
                column: "SignUpUserUserId",
                principalTable: "DbUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
