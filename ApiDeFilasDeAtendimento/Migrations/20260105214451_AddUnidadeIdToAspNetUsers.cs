using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeFilasDeAtendimento.Migrations
{
    /// <inheritdoc />
    public partial class AddUnidadeIdToAspNetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UnidadeId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FilaSenha_UnidadeId",
                table: "FilaSenha",
                column: "UnidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UnidadeId",
                table: "AspNetUsers",
                column: "UnidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Unidade_UnidadeId",
                table: "AspNetUsers",
                column: "UnidadeId",
                principalTable: "Unidade",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FilaSenha_Unidade_UnidadeId",
                table: "FilaSenha",
                column: "UnidadeId",
                principalTable: "Unidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Unidade_UnidadeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FilaSenha_Unidade_UnidadeId",
                table: "FilaSenha");

            migrationBuilder.DropIndex(
                name: "IX_FilaSenha_UnidadeId",
                table: "FilaSenha");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UnidadeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UnidadeId",
                table: "AspNetUsers");
        }
    }
}
