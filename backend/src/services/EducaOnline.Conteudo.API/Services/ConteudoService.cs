using EducaOnline.Conteudo.API.Models;
using EducaOnline.Conteudo.API.Models.ValueObjects;
using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Conteudo.API.Services
{
    public class ConteudoService : IConteudoService
    {
        private readonly IConteudoRepository _conteudoRepository;

        public ConteudoService(IConteudoRepository conteudoRepository)
        {
            _conteudoRepository = conteudoRepository;
        }

        public async Task<List<Curso>> BuscarCursos()
        {
            return await _conteudoRepository.BuscarCursos();
        }

        public async Task<Curso?> BuscarCurso(Guid id)
        {
            return await _conteudoRepository.BuscarCurso(id);
        }

        public async Task AdicionarCurso(Curso curso)
        {
            curso.Validar();
            _conteudoRepository.AdicionarCurso(curso);

            await _conteudoRepository.UnitOfWork.Commit();
        }

        public async Task<Curso> AlterarCurso(Guid id, Curso model)
        {
            var curso = await _conteudoRepository.BuscarCurso(id);
            if (curso is null)
                throw new DomainException("Curso não encontrado");

            curso.Atualizar(model);

            _conteudoRepository.AlterarCurso(curso);

            await _conteudoRepository.UnitOfWork.Commit();

            return curso;
        }

        public async Task<Curso> AlterarConteudoProgramaticoCurso(Guid id, ConteudoProgramatico conteudoProgramatico)
        {
            var curso = await _conteudoRepository.BuscarCurso(id);
            if (curso is null)
                throw new DomainException("Curso não encontrado");

            curso.AtualizarConteudoProgramatico(conteudoProgramatico);

            _conteudoRepository.AlterarCurso(curso);

            await _conteudoRepository.UnitOfWork.Commit();

            return curso;
        }

        public async Task DesativarCurso(Guid id)
        {
            var curso = await _conteudoRepository.BuscarCurso(id);
            if (curso is null)
                throw new DomainException("Curso não encontrado");

            curso.Desativar();
            _conteudoRepository.AlterarCurso(curso);
            await _conteudoRepository.UnitOfWork.Commit();
        }

        public async Task<int> ObterTotalAulasPorCurso(Guid cursoId)
        {
            var curso = await _conteudoRepository.BuscarCurso(cursoId);

            if (curso is null)
                throw new DomainException("Curso não encontrado");

            return curso.Aulas!.Count();
        }

        public async Task<int> ObterHorasAulasPorCurso(Guid cursoId, Guid aulaId)
        {
            var curso = await _conteudoRepository.BuscarCurso(cursoId);

            if (curso is null)
                throw new DomainException("Curso não encontrado");

            var aula = curso.Aulas!.FirstOrDefault(p => p.Id == aulaId);

            if (aula is null)
                throw new DomainException("Aula não encontrada");

            return aula.TotalHoras;
        }

        public async Task<Curso> AdicionarAula(Guid cursoId, Aula aula)
        {
            var curso = await _conteudoRepository.BuscarCurso(cursoId);
            if (curso is null)
                throw new DomainException("Curso não encontrado");

            curso.AdicionarAula(aula);

            _conteudoRepository.AlterarCurso(curso);
            await _conteudoRepository.UnitOfWork.Commit();
            return curso;
        }

        public async Task<Curso> AlterarAula(Guid cursoId, Guid aulaId, Aula aula)
        {
            var curso = await _conteudoRepository.BuscarCurso(cursoId);
            if (curso is null)
                throw new DomainException("Curso não encontrado");

            curso.AlterarAula(aulaId, aula);

            _conteudoRepository.AlterarCurso(curso);

            await _conteudoRepository.UnitOfWork.Commit();
            return curso;
        }

        public async Task<Curso> RemoverAula(Guid cursoId, Guid aulaId)
        {
            var curso = await _conteudoRepository.BuscarCurso(cursoId);
            if (curso is null)
                throw new DomainException("Curso não encontrado");

            curso.RemoverAula(aulaId);

            _conteudoRepository.AlterarCurso(curso);

            await _conteudoRepository.UnitOfWork.Commit();
            return curso;
        }
    }
}
