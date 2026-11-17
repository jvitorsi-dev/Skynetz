namespace Skynetz.Application.Simulacoes.Commands

{
    public class CalcularSimulacaoCommand
    {
        public string DddOrigem { get; private set; }
        public string DddDestino { get; private set; }
        public int Minutos { get; private set; }
        public string NomePlano { get; private set; }

        public CalcularSimulacaoCommand(
            string dddOrigem,
            string dddDestino,
            int minutos,
            string nomePlano)
        {
            DddOrigem = dddOrigem;
            DddDestino = dddDestino;
            Minutos = minutos;
            NomePlano = nomePlano;
        }
    }
}