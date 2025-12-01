using EducaOnline.Core.Data;

namespace EducaOnline.Conteudo.API.Models
{
    public interface IConteudoRepository : IRepository<Curso>
    {
        void AdicionarCurso(Curso curso);
        void AlterarCurso(Curso curso);
        Task<List<Curso>> BuscarCursos();
        Task<Curso?> BuscarCurso(Guid id);
        void RemoverCurso(Curso curso);
    }
}
