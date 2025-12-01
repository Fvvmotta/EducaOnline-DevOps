namespace EducaOnline.Financeiro.API.Models
{
    public class CartaoCredito
    {
        public CartaoCredito(string? nomeCartao, string? numeroCartao, string? mesAnoVencimento, string? cvv)
        {
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            MesAnoVencimento = mesAnoVencimento;
            CVV = cvv;
        }
        public string? NomeCartao { get; private set; }
        public string? NumeroCartao { get; private set; }
        public string? MesAnoVencimento { get; private set; }
        public string? CVV { get; private set; }
    }
}
