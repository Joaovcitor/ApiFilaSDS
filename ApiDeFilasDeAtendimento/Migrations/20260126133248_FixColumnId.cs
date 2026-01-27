using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeFilasDeAtendimento.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilaSenha_TiposDeAtendimento_TiposDeAtendimentoId",
                table: "FilaSenha");

            migrationBuilder.DropIndex(
                name: "IX_FilaSenha_TiposDeAtendimentoId",
                table: "FilaSenha");

            migrationBuilder.DropColumn(
                name: "TipoDeAtendimento",
                table: "FilaSenha");

            migrationBuilder.DropColumn(
                name: "TiposDeAtendimentoId",
                table: "FilaSenha");

            migrationBuilder.CreateIndex(
                name: "IX_FilaSenha_TipoAtendimentoId",
                table: "FilaSenha",
                column: "TipoAtendimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilaSenha_TiposDeAtendimento_TipoAtendimentoId",
                table: "FilaSenha",
                column: "TipoAtendimentoId",
                principalTable: "TiposDeAtendimento",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilaSenha_TiposDeAtendimento_TipoAtendimentoId",
                table: "FilaSenha");

            migrationBuilder.DropIndex(
                name: "IX_FilaSenha_TipoAtendimentoId",
                table: "FilaSenha");

            migrationBuilder.AddColumn<int>(
                name: "TipoDeAtendimento",
                table: "FilaSenha",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TiposDeAtendimentoId",
                table: "FilaSenha",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FilaSenha_TiposDeAtendimentoId",
                table: "FilaSenha",
                column: "TiposDeAtendimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilaSenha_TiposDeAtendimento_TiposDeAtendimentoId",
                table: "FilaSenha",
                column: "TiposDeAtendimentoId",
                principalTable: "TiposDeAtendimento",
                principalColumn: "Id");
        }
    }
}
