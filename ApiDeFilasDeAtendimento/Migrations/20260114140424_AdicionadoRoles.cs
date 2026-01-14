using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeFilasDeAtendimento.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadoRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guiche_AspNetUsers_DonoId",
                table: "Guiche");

            migrationBuilder.DropForeignKey(
                name: "FK_Unidade_AspNetUsers_DonoId",
                table: "Unidade");

            migrationBuilder.AlterColumn<string>(
                name: "Local",
                table: "Unidade",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DonoId",
                table: "Unidade",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Guiche",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DonoId",
                table: "Guiche",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "FilaSenha",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NomeCompleto",
                table: "AspNetUsers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Guiche_AspNetUsers_DonoId",
                table: "Guiche",
                column: "DonoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Unidade_AspNetUsers_DonoId",
                table: "Unidade",
                column: "DonoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "FilaSenha");

            migrationBuilder.AlterColumn<string>(
                name: "Local",
                table: "Unidade",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "DonoId",
                table: "Unidade",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Guiche",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "DonoId",
                table: "Guiche",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NomeCompleto",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

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
    }
}
