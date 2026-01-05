using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeFilasDeAtendimento.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGuicheIdUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guiche_AspNetUsers_FuncionarioId1",
                table: "Guiche");

            migrationBuilder.DropIndex(
                name: "IX_Guiche_FuncionarioId1",
                table: "Guiche");

            migrationBuilder.DropColumn(
                name: "FuncionarioId1",
                table: "Guiche");

            migrationBuilder.DropColumn(
                name: "GuicheId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Guiche_FuncionarioId",
                table: "Guiche",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guiche_AspNetUsers_FuncionarioId",
                table: "Guiche",
                column: "FuncionarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guiche_AspNetUsers_FuncionarioId",
                table: "Guiche");

            migrationBuilder.DropIndex(
                name: "IX_Guiche_FuncionarioId",
                table: "Guiche");

            migrationBuilder.AddColumn<string>(
                name: "FuncionarioId1",
                table: "Guiche",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuicheId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Guiche_FuncionarioId1",
                table: "Guiche",
                column: "FuncionarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Guiche_AspNetUsers_FuncionarioId1",
                table: "Guiche",
                column: "FuncionarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
