using EducaOnline.Conteudo.API.Models.ValueObjects;
using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Conteudo.API.Models
{
    public class Curso : Entity, IAggregateRoot
    {
        public Curso()
        {
            Aulas = new HashSet<Aula>();
        }

        public Curso(string? nome, ConteudoProgramatico? conteudoProgramatico, bool ativo, decimal valor)
        {
            Nome = nome;
            ConteudoProgramatico = conteudoProgramatico;
            Aulas = new HashSet<Aula>();
            Ativo = ativo;
            Valor = valor;
        }
        public Curso(Guid id, string nome, ConteudoProgramatico conteudoProgramatico, bool ativo, decimal valor)
            : this(nome, conteudoProgramatico, ativo, valor)
        {
            Id = id; 
        }

        public string? Nome { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; set; }

        public ConteudoProgramatico? ConteudoProgramatico { get; private set; }
        public ICollection<Aula>? Aulas { get; private set; }


        public void Atualizar(Curso curso)
        {
            Validacoes.ValidarSeVazio(Nome!, "O campo Nome do curso não pode estar vazio");

            Nome = curso.Nome;
            Ativo = curso.Ativo;

            AtualizarConteudoProgramatico(curso.ConteudoProgramatico!);
        }

        public void AdicionarAula(Aula aula)
        {
            if (Aulas == null)
                Aulas = new List<Aula>();

            Aulas.Add(aula);

            ConteudoProgramatico?.AtualizarCargaHoraria(Aulas.Sum(p => p.TotalHoras));
        }

        public void AlterarAula(Guid aulaId, Aula aula)
        {
            foreach (var aulaDomain in Aulas!.Where(p => p.Id == aulaId))
                aulaDomain.Atualizar(aula);

            ConteudoProgramatico?.AtualizarCargaHoraria(Aulas!.Sum(p => p.TotalHoras));
        }

        public void RemoverAula(Guid aulaId)
        {
            var aula = Aulas?.FirstOrDefault(p => p.Id == aulaId);

            if (aula is null)
                throw new DomainException("Aula não encontrada");

            Aulas!.Remove(aula);

            ConteudoProgramatico?.AtualizarCargaHoraria(Aulas!.Sum(p => p.TotalHoras));
        }

        public void AtualizarConteudoProgramatico(ConteudoProgramatico conteudoProgramatico)
        {

            ConteudoProgramatico = conteudoProgramatico;
            ValidarConteudoProgramtico();
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome!, "O campo Nome do curso não pode estar vazio");
            ValidarConteudoProgramtico();
        }

        public void ValidarConteudoProgramtico()
        {
            Validacoes.ValidarSeVazio(ConteudoProgramatico!.Titulo!, "O campo Titulo do conteudo programático não pode estar vazio");
            Validacoes.ValidarSeVazio(ConteudoProgramatico.Descricao!, "O campo Descricao do conteudo programático não pode estar vazio");
            Validacoes.ValidarMinimoMaximo(ConteudoProgramatico.CargaHoraria, 1, 2000, "O campo CargaHoraria do conteudo programático deve estar dentro de 1 hora até 2000 horas");
            Validacoes.ValidarSeVazio(ConteudoProgramatico.Objetivos!, "O campo Objetivos do conteudo programático não pode estar vazio");
            Validacoes.ValidarSeMenorQue(Valor, 0.01M, "O campo Valor do curso não pode ser menor que 0.01");
        }

        public void Desativar()
        {
            Ativo = false;
        }
    }
}
