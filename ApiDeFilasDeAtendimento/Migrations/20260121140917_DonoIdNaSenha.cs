using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeFilasDeAtendimento.Migrations
{
    /// <inheritdoc />
    public partial class DonoIdNaSenha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DonoId",
                table: "FilaSenha",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FilaSenha_DonoId",
                table: "FilaSenha",
                column: "DonoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilaSenha_AspNetUsers_DonoId",
                table: "FilaSenha",
                column: "DonoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilaSenha_AspNetUsers_DonoId",
                table: "FilaSenha");

            migrationBuilder.DropIndex(
                name: "IX_FilaSenha_DonoId",
                table: "FilaSenha");

            migrationBuilder.DropColumn(
                name: "DonoId",
                table: "FilaSenha");
        }
    }
}
