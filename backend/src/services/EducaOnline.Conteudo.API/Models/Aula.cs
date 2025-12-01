using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Conteudo.API.Models
{
    public class Aula : Entity
    {
        protected Aula()
        {
        }

        public Aula(string titulo, string descricao, int totalHoras)
        {
            Titulo = titulo;
            Descricao = descricao;
            TotalHoras = totalHoras;
        }

        public string? Titulo { get; private set; }
        public string? Descricao { get; private set; }
        public int TotalHoras { get; private set; }

        public Guid CursoId { get; private set; }
        public Curso? Curso { get; private set; }


        public void Atualizar(Aula aula)
        {
            Titulo = aula.Titulo;
            Descricao = aula.Descricao;
            TotalHoras = aula.TotalHoras;
        }
    }
}
