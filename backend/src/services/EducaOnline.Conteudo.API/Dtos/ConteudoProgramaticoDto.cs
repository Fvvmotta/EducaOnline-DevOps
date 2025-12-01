namespace EducaOnline.Conteudo.API.Dtos
{
    public class ConteudoProgramaticoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int CargaHoraria { get; set; }
        public string Objetivos { get; set; } = string.Empty;
    }
}
