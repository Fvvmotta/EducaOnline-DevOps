namespace EducaOnline.Financeiro.Pagamentos
{
    public class PagamentoService
    {
        public readonly string ApiKey;
        public readonly string EncryptionKey;

        public PagamentoService(string apiKey, string encryptionKey)
        {
            ApiKey = apiKey;
            EncryptionKey = encryptionKey;
        }
    }
}
