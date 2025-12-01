using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EducaOnline.Bff.Models
{
    public class MatricularAlunoViewModel
    {
        [Required(ErrorMessage = "Informe o Id do Curso")]        
        public Guid? CursoId { get; set; }
    }
}
