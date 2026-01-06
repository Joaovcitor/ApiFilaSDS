using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeFilasDeAtendimento.Migrations
{
    /// <inheritdoc />
    public partial class AdicioneiCollectionsDeSenhas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FilaSenha_GuicheId",
                table: "FilaSenha",
                column: "GuicheId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilaSenha_Guiche_GuicheId",
                table: "FilaSenha",
                column: "GuicheId",
                principalTable: "Guiche",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilaSenha_Guiche_GuicheId",
                table: "FilaSenha");

            migrationBuilder.DropIndex(
                name: "IX_FilaSenha_GuicheId",
                table: "FilaSenha");
        }
    }
}
