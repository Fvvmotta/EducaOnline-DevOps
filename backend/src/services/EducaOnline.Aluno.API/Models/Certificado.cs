using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Aluno.API.Models
{
    public class Certificado : Entity
    {
        protected Certificado()
        {

        }
        public Certificado(string curso)
        {
            Id = Guid.Empty;
            Curso = curso;
            DataEmissao = DateTime.UtcNow;
            Numero = $"Cert-{new Random().Next(1, 99999)}";
        }

        public string? Curso { get; private set; }
        public string? Numero { get; private set; }
        public DateTime DataEmissao { get; private set; }

        public Guid AlunoId { get; private set; }
        public Aluno? Aluno { get; private set; }
    }
}
