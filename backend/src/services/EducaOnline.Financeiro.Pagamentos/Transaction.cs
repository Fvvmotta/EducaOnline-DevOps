namespace EducaOnline.Financeiro.Pagamentos
{
    public class Transaction
    {
        private readonly PagamentoService? PagamentoService;

        protected Transaction(){}
        public Transaction(PagamentoService pagamentoService)
        {
            PagamentoService = pagamentoService;
        }

        public TransactionStatus Status { get; set; }

        public string? CardHash { get; set; }

        public string? CardNumber { get; set; }

        public string? CardExpirationDate { get; set; }

        public string? AuthorizationCode { get; set; }
        public string? Tid { get; set; }
        public string? Nsu { get; set; }

        public decimal Amount { get; set; }
        public decimal Cost { get; set; }

        public string? CardHolderName { get; set; }

        public string? CardCvv { get; set; }
        public string? CardBrand { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; }

        public Task<Transaction> AuthorizeCardTransaction()
        {
            var success = new Random().Next(2) == 0;
            Transaction transaction;

            if (success)
            {
                transaction = new Transaction
                {
                    AuthorizationCode = GetGenericCode(),
                    CardBrand = "MasterCard",
                    TransactionDate = DateTime.Now,
                    Cost = Amount * (decimal)0.03,
                    Amount = Amount,
                    Status = TransactionStatus.Authorized,
                    Tid = GetGenericCode(),
                    Nsu = GetGenericCode()
                };

                return Task.FromResult(transaction);
            }

            transaction = new Transaction
            {
                AuthorizationCode = "",
                CardBrand = "",
                TransactionDate = DateTime.Now,
                Cost = 0,
                Amount = 0,
                Status = TransactionStatus.Refused,
                Tid = "",
                Nsu = ""
            };

            return Task.FromResult(transaction);
        }

        public Task<Transaction> CaptureCardTransaction()
        {
            var transaction = new Transaction
            {
                AuthorizationCode = GetGenericCode(),
                CardBrand = CardBrand,
                TransactionDate = DateTime.Now,
                Cost = 0,
                Amount = Amount,
                Status = TransactionStatus.Paid,
                Tid = Tid,
                Nsu = Nsu
            };

            return Task.FromResult(transaction);
        }

        public Task<Transaction> CancelAuthorization()
        {
            var transaction = new Transaction
            {
                AuthorizationCode = "",
                CardBrand = CardBrand,
                TransactionDate = DateTime.Now,
                Cost = 0,
                Amount = Amount,
                Status = TransactionStatus.Cancelled,
                Tid = Tid,
                Nsu = Nsu
            };

            return Task.FromResult(transaction);
        }

        private string GetGenericCode()
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
