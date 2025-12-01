namespace EducaOnline.Aluno.API.DTO
{
    public class PagamentoMatriculaDto
    {
        public string? NomeCartao { get; set; }
        public string? NumeroCartao { get; set; }
        public string? ExpiracaoCartao { get; set; }
        public string? CvvCartao { get; set; }
        public decimal ValorCurso { get; set; }
    }
}
