namespace EducaOnline.Conteudo.API.Dtos
{
    public class AdicionarCursoDto
    {
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public decimal Valor { get; set; }
        public ConteudoProgramaticoDto ConteudoProgramatico { get; set; } = new();
    }
}
