using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducaOnline.Aluno.API.Migrations
{
    /// <inheritdoc />
    public partial class Teste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AulaConcluida_Aluno_AlunoId",
                table: "AulaConcluida");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificado_Aluno_AlunoId",
                table: "Certificado");

            migrationBuilder.DropForeignKey(
                name: "FK_Matricula_Aluno_AlunoId",
                table: "Matricula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matricula",
                table: "Matricula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificado",
                table: "Certificado");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AulaConcluida",
                table: "AulaConcluida");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aluno",
                table: "Aluno");

            migrationBuilder.RenameTable(
                name: "Matricula",
                newName: "Matriculas");

            migrationBuilder.RenameTable(
                name: "Certificado",
                newName: "Certificados");

            migrationBuilder.RenameTable(
                name: "AulaConcluida",
                newName: "AulaConcluidas");

            migrationBuilder.RenameTable(
                name: "Aluno",
                newName: "Alunos");

            migrationBuilder.RenameIndex(
                name: "IX_Matricula_AlunoId",
                table: "Matriculas",
                newName: "IX_Matriculas_AlunoId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificado_AlunoId",
                table: "Certificados",
                newName: "IX_Certificados_AlunoId");

            migrationBuilder.RenameIndex(
                name: "IX_AulaConcluida_AlunoId",
                table: "AulaConcluidas",
                newName: "IX_AulaConcluidas_AlunoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matriculas",
                table: "Matriculas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificados",
                table: "Certificados",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AulaConcluidas",
                table: "AulaConcluidas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alunos",
                table: "Alunos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AulaConcluidas_Alunos_AlunoId",
                table: "AulaConcluidas",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificados_Alunos_AlunoId",
                table: "Certificados",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Alunos_AlunoId",
                table: "Matriculas",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AulaConcluidas_Alunos_AlunoId",
                table: "AulaConcluidas");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificados_Alunos_AlunoId",
                table: "Certificados");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Alunos_AlunoId",
                table: "Matriculas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matriculas",
                table: "Matriculas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificados",
                table: "Certificados");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AulaConcluidas",
                table: "AulaConcluidas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alunos",
                table: "Alunos");

            migrationBuilder.RenameTable(
                name: "Matriculas",
                newName: "Matricula");

            migrationBuilder.RenameTable(
                name: "Certificados",
                newName: "Certificado");

            migrationBuilder.RenameTable(
                name: "AulaConcluidas",
                newName: "AulaConcluida");

            migrationBuilder.RenameTable(
                name: "Alunos",
                newName: "Aluno");

            migrationBuilder.RenameIndex(
                name: "IX_Matriculas_AlunoId",
                table: "Matricula",
                newName: "IX_Matricula_AlunoId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificados_AlunoId",
                table: "Certificado",
                newName: "IX_Certificado_AlunoId");

            migrationBuilder.RenameIndex(
                name: "IX_AulaConcluidas_AlunoId",
                table: "AulaConcluida",
                newName: "IX_AulaConcluida_AlunoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matricula",
                table: "Matricula",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificado",
                table: "Certificado",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AulaConcluida",
                table: "AulaConcluida",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aluno",
                table: "Aluno",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AulaConcluida_Aluno_AlunoId",
                table: "AulaConcluida",
                column: "AlunoId",
                principalTable: "Aluno",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificado_Aluno_AlunoId",
                table: "Certificado",
                column: "AlunoId",
                principalTable: "Aluno",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matricula_Aluno_AlunoId",
                table: "Matricula",
                column: "AlunoId",
                principalTable: "Aluno",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
