using System.ComponentModel.DataAnnotations;

namespace EducaOnline.Identidade.API.Models
{
    public class UsuarioRegistro
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string? Nome { get; set; }

        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres",MinimumLength = 6)]        
        public string? Senha { get; set; }

        [Compare(nameof(Senha), ErrorMessage = "As senhas não conferem")]
        public string? SenhaConfirmacao { get; set; }
    }

    public class UsuarioLogin
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string? Senha { get; set; }
    }

    public class UsuarioRespostaLogin
    {
        public string? AccessToken { get; internal set; }
        public double ExpiresIn { get; internal set; }
        public UsuarioToken? UserToken { get; internal set; }
    }

    public class UsuarioToken
    {
        public string? Id { get; internal set; }
        public string? Email { get; internal set; }
        public string? Nome { get; internal set; }
        public object? Claims { get; internal set; }
    }

    public class UsuarioClaim
    {
        public string? Type { get; internal set; }
        public string? Value { get; internal set; }
    }
}
