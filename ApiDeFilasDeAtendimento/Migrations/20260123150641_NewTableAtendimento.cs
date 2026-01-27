using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeFilasDeAtendimento.Migrations
{
    /// <inheritdoc />
    public partial class NewTableAtendimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TipoAtendimentoId",
                table: "FilaSenha",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.AddColumn<Guid>(
                name: "TipoAtendimentoId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TiposDeAtendimentoId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TiposDeAtendimento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false),
                    DonoId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDeAtendimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiposDeAtendimento_AspNetUsers_DonoId",
                        column: x => x.DonoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilaSenha_TiposDeAtendimentoId",
                table: "FilaSenha",
                column: "TiposDeAtendimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TiposDeAtendimentoId",
                table: "AspNetUsers",
                column: "TiposDeAtendimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposDeAtendimento_DonoId",
                table: "TiposDeAtendimento",
                column: "DonoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TiposDeAtendimento_TiposDeAtendimentoId",
                table: "AspNetUsers",
                column: "TiposDeAtendimentoId",
                principalTable: "TiposDeAtendimento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FilaSenha_TiposDeAtendimento_TiposDeAtendimentoId",
                table: "FilaSenha",
                column: "TiposDeAtendimentoId",
                principalTable: "TiposDeAtendimento",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TiposDeAtendimento_TiposDeAtendimentoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FilaSenha_TiposDeAtendimento_TiposDeAtendimentoId",
                table: "FilaSenha");

            migrationBuilder.DropTable(
                name: "TiposDeAtendimento");

            migrationBuilder.DropIndex(
                name: "IX_FilaSenha_TiposDeAtendimentoId",
                table: "FilaSenha");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TiposDeAtendimentoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TipoAtendimentoId",
                table: "FilaSenha");

            migrationBuilder.DropColumn(
                name: "TipoDeAtendimento",
                table: "FilaSenha");

            migrationBuilder.DropColumn(
                name: "TiposDeAtendimentoId",
                table: "FilaSenha");

            migrationBuilder.DropColumn(
                name: "TipoAtendimentoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TiposDeAtendimentoId",
                table: "AspNetUsers");
        }
    }
}
