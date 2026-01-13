using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeFilasDeAtendimento.Migrations
{
    /// <inheritdoc />
    public partial class AddModelDeRelacionamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Unidade_UnidadeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UnidadeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UnidadeId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LocalId",
                table: "AspNetUsers",
                column: "LocalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Unidade_LocalId",
                table: "AspNetUsers",
                column: "LocalId",
                principalTable: "Unidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Unidade_LocalId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LocalId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "UnidadeId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

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
        }
    }
}
