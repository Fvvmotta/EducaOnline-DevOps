using EducaOnline.Conteudo.API.Models;
using EducaOnline.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EducaOnline.Conteudo.API.Data.Repository
{
    public class ConteudoRepository : IConteudoRepository
    {
        private readonly ConteudoContext _context;

        public ConteudoRepository(ConteudoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Curso?> BuscarCurso(Guid id) => await _context.Cursos.Include(p => p.Aulas).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Curso>> BuscarCursos() => await _context.Cursos.Include(p => p.Aulas).ToListAsync();

        public void AdicionarCurso(Curso curso) => _context.Cursos.Add(curso);

        public void AlterarCurso(Curso curso) => _context.Cursos.Update(curso);

        public void RemoverCurso(Curso curso) => _context.Cursos.Remove(curso);

    }
}
