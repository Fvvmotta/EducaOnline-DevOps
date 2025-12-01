using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Aluno.API.Models.ValueObjects
{
    public class HistoricoAprendizado
    {
        public int? TotalAulasConcluidas { get; private set; }
        public int? TotalAulas { get; private set; }
        public double? Progresso { get; private set; } 

        private HistoricoAprendizado() { }

        public HistoricoAprendizado(int totalAulasConcluidas, int totalAulas)
        {
            if (totalAulas < 0) throw new DomainException("Total de aulas inválido.");
            if (totalAulasConcluidas < 0) throw new DomainException("Total de aulas concluídas inválido.");
            if (totalAulasConcluidas > totalAulas) totalAulasConcluidas = totalAulas;

            TotalAulasConcluidas = totalAulasConcluidas;
            TotalAulas = totalAulas;
            Progresso = CalcularProgresso(TotalAulasConcluidas.Value, TotalAulas.Value);
        }

        public void IncrementarAulaConcluida()
        {
            if (TotalAulas == 0)
                throw new DomainException("Total de aulas não informado.");

            if (TotalAulasConcluidas < TotalAulas)
            {
                TotalAulasConcluidas += 1;
                Progresso = CalcularProgresso(TotalAulasConcluidas.Value, TotalAulas.Value);
            }
        }

        private static double CalcularProgresso(int concluidas, int total)
        {
            if (total <= 0) return 0;
            return Math.Round((double)((decimal)concluidas / total * 100m), 0, MidpointRounding.AwayFromZero);
        }
        public HistoricoAprendizado Atualizar(int concluidas, int total)
        {
            return new HistoricoAprendizado(concluidas, total);
        }

        public void AtualizarProgresso(int totalAulasConcluidas, int totalAulas)
        {
            if (totalAulas < 0) throw new DomainException("Total de aulas inválido.");
            if (totalAulasConcluidas < 0) throw new DomainException("Total de aulas concluídas inválido.");
            if (totalAulasConcluidas > totalAulas) totalAulasConcluidas = totalAulas;

            TotalAulasConcluidas = totalAulasConcluidas;
            TotalAulas = totalAulas;
            Progresso = CalcularProgresso(totalAulasConcluidas, totalAulas);
        }


    }
}
