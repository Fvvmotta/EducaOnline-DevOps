using EducaOnline.Conteudo.API.Dtos;
using EducaOnline.Conteudo.API.Models;
using EducaOnline.Conteudo.API.Models.ValueObjects;
using EducaOnline.Conteudo.API.Services;
using EducaOnline.Core.Enums;
using EducaOnline.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducaOnline.Conteudo.API.Controllers
{
    [Authorize]
    [Route("api/conteudos")]
    public class ConteudoController : MainController
    {
        private readonly IConteudoService _conteudoService;

        public ConteudoController(IConteudoService conteudoService)
        {
            _conteudoService = conteudoService;
        }

        /// <summary>
        /// Lista os cursos
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<Curso>), 200)]
        public async Task<IActionResult> Get()
        {
            var cursos = await _conteudoService.BuscarCursos();
            if (cursos == null || !cursos.Any())
                AdicionarErro("Nenhum curso encontrado.");

            return CustomResponse(cursos);
        }

        /// <summary>
        /// Busca um curso específico por id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Curso), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            var curso = await _conteudoService.BuscarCurso(id);
            if (curso == null)
            {
                AdicionarErro("Curso não encontrado.");
                return CustomResponse();
            }

            return CustomResponse(curso);
        }

        /// <summary>
        /// Adicionar um novo curso
        /// </summary>
        [Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpPost]
        public async Task<IActionResult> AdicionarCurso([FromBody] AdicionarCursoDto model)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var conteudoProgramatico = new ConteudoProgramatico(
                model.ConteudoProgramatico.Titulo,
                model.ConteudoProgramatico.Descricao,
                model.ConteudoProgramatico.CargaHoraria,
                model.ConteudoProgramatico.Objetivos
            );

            var curso = new Curso(
                model.Nome,
                conteudoProgramatico,
                model.Ativo,
                model.Valor
            );

            try
            {
                await _conteudoService.AdicionarCurso(curso);
            }
            catch (Exception ex)
            {
                AdicionarErro(ex.Message);
                return CustomResponse();
            }

            return CustomResponse(new { message = "Curso adicionado com sucesso." });
        }

        /// <summary>
        /// Atualiza os dados de um curso específico
        /// </summary>
        [Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarCurso(Guid id, [FromBody] AtualizarCursoDto model)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            try
            {
                var conteudoProgramatico = new ConteudoProgramatico(
                    model.ConteudoProgramatico.Titulo,
                    model.ConteudoProgramatico.Descricao,
                    model.ConteudoProgramatico.CargaHoraria,
                    model.ConteudoProgramatico.Objetivos
                );

                var curso = new Curso(
                    model.Nome,
                    conteudoProgramatico,
                    model.Ativo,
                    model.Valor
                );

                await _conteudoService.AlterarCurso(id, curso);
            }
            catch (Exception ex)
            {
                AdicionarErro(ex.Message);
                return CustomResponse();
            }

            return CustomResponse(new { message = "Curso atualizado com sucesso." });
        }

        /// <summary>
        /// Desativa um curso
        /// </summary>
        [Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpPut("{id:guid}/desativar")]
        public async Task<IActionResult> DesativarCurso(Guid id)
        {
            try
            {
                await _conteudoService.DesativarCurso(id);
            }
            catch (Exception ex)
            {
                AdicionarErro(ex.Message);
                return CustomResponse();
            }

            return CustomResponse(new { message = "Curso desativado com sucesso." });
        }

        /// <summary>
        /// Adiciona uma aula ao curso informado
        /// </summary>
        [Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpPost("{id:guid}/aula")]
        [ProducesResponseType(typeof(Curso), 200)]
        public async Task<IActionResult> AdicionarAula(Guid id, [FromBody] Aula aula)
        {
            try
            {
                var curso = await _conteudoService.AdicionarAula(id, aula);
                return CustomResponse(curso);
            }
            catch (Exception ex)
            {
                AdicionarErro(ex.Message);
                return CustomResponse();
            }
        }

        /// <summary>
        /// Altera uma aula do curso informado
        /// </summary>
        [Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpPut("{id:guid}/aula/{aulaId:guid}")]
        [ProducesResponseType(typeof(Curso), 200)]
        public async Task<IActionResult> AlterarAula(Guid id, Guid aulaId, [FromBody] Aula aula)
        {
            try
            {
                var curso = await _conteudoService.AlterarAula(id, aulaId, aula);
                return CustomResponse(curso);
            }
            catch (Exception ex)
            {
                AdicionarErro(ex.Message);
                return CustomResponse();
            }
        }

        /// <summary>
        /// Remove uma aula do curso informado
        /// </summary>
        [Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpDelete("{id:guid}/aula/{aulaId:guid}")]
        [ProducesResponseType(typeof(Curso), 200)]
        public async Task<IActionResult> RemoverAula(Guid id, Guid aulaId)
        {
            try
            {
                var curso = await _conteudoService.RemoverAula(id, aulaId);
                return CustomResponse(curso);
            }
            catch (Exception ex)
            {
                AdicionarErro(ex.Message);
                return CustomResponse();
            }
        }
    }
}
