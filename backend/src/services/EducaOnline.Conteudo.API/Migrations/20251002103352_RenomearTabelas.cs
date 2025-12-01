using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducaOnline.Conteudo.API.Migrations
{
    /// <inheritdoc />
    public partial class RenomearTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aula_Curso_CursoId",
                table: "Aula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Curso",
                table: "Curso");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aula",
                table: "Aula");

            migrationBuilder.RenameTable(
                name: "Curso",
                newName: "Cursos");

            migrationBuilder.RenameTable(
                name: "Aula",
                newName: "Aulas");

            migrationBuilder.RenameIndex(
                name: "IX_Aula_CursoId",
                table: "Aulas",
                newName: "IX_Aulas_CursoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cursos",
                table: "Cursos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aulas",
                table: "Aulas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aulas_Cursos_CursoId",
                table: "Aulas",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aulas_Cursos_CursoId",
                table: "Aulas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cursos",
                table: "Cursos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aulas",
                table: "Aulas");

            migrationBuilder.RenameTable(
                name: "Cursos",
                newName: "Curso");

            migrationBuilder.RenameTable(
                name: "Aulas",
                newName: "Aula");

            migrationBuilder.RenameIndex(
                name: "IX_Aulas_CursoId",
                table: "Aula",
                newName: "IX_Aula_CursoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Curso",
                table: "Curso",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aula",
                table: "Aula",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aula_Curso_CursoId",
                table: "Aula",
                column: "CursoId",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
