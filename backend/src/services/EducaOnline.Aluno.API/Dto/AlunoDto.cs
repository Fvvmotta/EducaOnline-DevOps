using System.ComponentModel.DataAnnotations;

namespace EducaOnline.Aluno.API.DTO
{
    public class AlunoDto
    {
        public string? Nome { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;
    }
}
