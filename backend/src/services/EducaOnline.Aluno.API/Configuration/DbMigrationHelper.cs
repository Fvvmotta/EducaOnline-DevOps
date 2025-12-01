using EducaOnline.Aluno.API.Data;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Aluno.API.Models.Enum;
using EducaOnline.Core.Utils;
using EducaOnline.WebAPI.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EducaOnline.Aluno.API.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication app)
        {
            var services = app.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var alunoContext = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();
            await DbHealthChecker.TestConnection(alunoContext);

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            {
                await alunoContext.Database.EnsureCreatedAsync();

                var alunoId = Guid.Parse("40640fec-5daf-4956-b1c0-2fde87717b66");

                var cursoIA = Guid.Parse("04effc8b-fa4a-415c-90eb-95cdfdaba1b2");
                var cursoAngular = Guid.Parse("04effc8b-fa4a-415c-90eb-95cdfdaba1b7");
                var cursoDotNet = Guid.Parse("04effc8b-fa4a-415c-90eb-95cdfdaba1b8");

                var aluno = await alunoContext.Alunos
                    .Include(a => a.Matriculas)
                    .FirstOrDefaultAsync(a => a.Id == alunoId);

                if (aluno == null)
                {
                    aluno = new Models.Aluno(
                        alunoId,
                        nome: "Aluno Teste",
                        email: "aluno@educaonline.com.br"
                    );

                    aluno.VincularRa(10001);

                    var matriculaIA = new Matricula(aluno.Id, cursoIA, "Curso IA", totalAulas: 2, cargaHorariaTotal: 20);
                    var matriculaAngular = new Matricula(aluno.Id, cursoAngular, "Curso Angular", totalAulas: 2, cargaHorariaTotal: 20);

                    aluno.RealizarMatricula(matriculaIA);
                    aluno.RealizarMatricula(matriculaAngular);

                    alunoContext.Alunos.Add(aluno);
                    await alunoContext.SaveChangesAsync();
                }

                var matriculasExistentes = await alunoContext.Matriculas
                    .Where(m => m.AlunoId == alunoId)
                    .ToListAsync();

                if (!matriculasExistentes.Any(m => m.CursoId == cursoDotNet))
                {
                    var matriculaNet = new Matricula(alunoId, cursoDotNet, "Curso .NET", 2, 20);
                    alunoContext.Matriculas.Add(matriculaNet);
                    await alunoContext.SaveChangesAsync();
                }

                var primeiraMatricula = aluno.Matriculas.FirstOrDefault();
                if (primeiraMatricula != null)
                {
                    var aulaIdFake = Guid.NewGuid();
                    if (!alunoContext.AulasConcluidas.Any(a => a.AlunoId == aluno.Id))
                    {
                        var aulaConcluida = new AulaConcluida(aulaIdFake);
                        typeof(AulaConcluida).GetProperty(nameof(AulaConcluida.AlunoId))!
                            .SetValue(aulaConcluida, aluno.Id);

                        alunoContext.AulasConcluidas.Add(aulaConcluida);
                        await alunoContext.SaveChangesAsync();

                        aluno.AtualizarHistoricoAprendizado(1, primeiraMatricula.TotalAulas);
                        await alunoContext.SaveChangesAsync();
                    }
                }

                var certificadoExistente = alunoContext.Certificados.FirstOrDefault(c => c.AlunoId == aluno.Id);
                if (certificadoExistente == null)
                {
                    var certificado = new Certificado("Curso IA");
                    typeof(Certificado).GetProperty(nameof(Certificado.AlunoId))!
                        .SetValue(certificado, aluno.Id);

                    alunoContext.Certificados.Add(certificado);
                    await alunoContext.SaveChangesAsync();
                }
            }
        }
    }
}
