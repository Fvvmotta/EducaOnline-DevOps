using System.ComponentModel.DataAnnotations;

namespace EducaOnline.Conteudo.API.ViewModels;

public class CursoModel
{
    [Required(ErrorMessage = "Campo obrigatório")]
    public string? Nome { get; set; }
}