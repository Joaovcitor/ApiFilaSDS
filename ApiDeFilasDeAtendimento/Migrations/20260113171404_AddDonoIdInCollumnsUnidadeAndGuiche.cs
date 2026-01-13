using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeFilasDeAtendimento.Migrations
{
    /// <inheritdoc />
    public partial class AddDonoIdInCollumnsUnidadeAndGuiche : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DonoId",
                table: "Unidade",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DonoId",
                table: "Guiche",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unidade_DonoId",
                table: "Unidade",
                column: "DonoId");

            migrationBuilder.CreateIndex(
                name: "IX_Guiche_DonoId",
                table: "Guiche",
                column: "DonoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guiche_AspNetUsers_DonoId",
                table: "Guiche",
                column: "DonoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Unidade_AspNetUsers_DonoId",
                table: "Unidade",
                column: "DonoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guiche_AspNetUsers_DonoId",
                table: "Guiche");

            migrationBuilder.DropForeignKey(
                name: "FK_Unidade_AspNetUsers_DonoId",
                table: "Unidade");

            migrationBuilder.DropIndex(
                name: "IX_Unidade_DonoId",
                table: "Unidade");

            migrationBuilder.DropIndex(
                name: "IX_Guiche_DonoId",
                table: "Guiche");

            migrationBuilder.DropColumn(
                name: "DonoId",
                table: "Unidade");

            migrationBuilder.DropColumn(
                name: "DonoId",
                table: "Guiche");
        }
    }
}
