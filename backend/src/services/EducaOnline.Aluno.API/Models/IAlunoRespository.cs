using EducaOnline.Core.Data;

namespace EducaOnline.Aluno.API.Models
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        Task AdicionarAluno(Aluno aluno, CancellationToken cancellationToken);
        void AtualizarAluno(Aluno aluno);
        Task<IEnumerable<Aluno>> BuscarAlunos(CancellationToken cancellationToken);
        Task<Aluno?> BuscarAlunoPorRa(int ra, CancellationToken cancellationToken);
        Task<Aluno?> BuscarAlunoPorId(Guid id, CancellationToken cancellationToken);
        Task<Aluno?> BuscarPorEmail(string email, CancellationToken cancellationToken);
        Task<int> BuscarUltimoRa(CancellationToken cancellationToken);
        Task<int> BuscarProximoRa(CancellationToken cancellationToken);
        Task AdicionarMatricula(Matricula matricula, CancellationToken cancellationToken);
        void AtualizarMatricula(Matricula matricula);
        void AdicionarAulaConcluida(AulaConcluida aula);
        Task<Matricula?> BuscarMatriculaPorAlunoId(Guid alunoId, CancellationToken cancellationToken);
    }
}
