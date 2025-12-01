using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EducaOnline.Aluno.API.Data.Repository
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly AlunoDbContext _context;

        public AlunoRepository(AlunoDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AdicionarAluno(Models.Aluno aluno, CancellationToken cancellationToken)
        {
            await _context.Alunos.AddAsync(aluno, cancellationToken);
        }

        public async Task AdicionarMatricula(Matricula matricula, CancellationToken cancellationToken)
        {
            await _context.Matriculas.AddAsync(matricula, cancellationToken);
        }

        public void AtualizarAluno(Models.Aluno aluno)
        {
            _context.Alunos.Update(aluno);
        }

        public void AtualizarMatricula(Matricula matricula)
        {
            _context.Matriculas.Update(matricula);
        }
        public void AdicionarAulaConcluida(AulaConcluida aula)
        {
            _context.AulasConcluidas.Add(aula);
        }


        public async Task<int> BuscarUltimoRa(CancellationToken cancellationToken)
        {
            return await _context.Alunos.MaxAsync(p => p.Ra);
        }

        public async Task<IEnumerable<Models.Aluno>> BuscarAlunos(CancellationToken cancellationToken)
        {
            return await _context.Alunos
                .Include(p => p.Matriculas)
                .Include(p => p.AulasConcluidas)
                .Include(p => p.Certificados)
                .ToListAsync();
        }

        public async Task<Models.Aluno?> BuscarAlunoPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Alunos
                .Include(p => p.Matriculas)
                .Include(p => p.AulasConcluidas)
                .Include(p => p.Certificados)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Models.Aluno?> BuscarAlunoPorRa(int ra, CancellationToken cancellationToken = default)
        {
            return await _context.Alunos
                .Include(p => p.Matriculas)
                .Include(p => p.AulasConcluidas)
                .Include(p => p.Certificados)
                .FirstOrDefaultAsync(p => p.Ra == ra);
        }
        public async Task<Models.Aluno?> BuscarPorEmail(string email, CancellationToken cancellationToken)
        {
            return await _context.Alunos.FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
        }
        public async Task<int> BuscarProximoRa(CancellationToken cancellationToken = default)
        {
            var ultimoRa = await _context.Alunos.MaxAsync(p => (int?)p.Ra, cancellationToken);
            return ultimoRa.HasValue ? ultimoRa.Value + 1 : 10000; //está correto fazer essa verificação aqui e já retornar o valor certo?
        }

        public async Task<Matricula?> BuscarMatriculaPorAlunoId(Guid alunoId, CancellationToken cancellationToken)
        {
            return await _context.Matriculas
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId, cancellationToken);
        }
    }
}
