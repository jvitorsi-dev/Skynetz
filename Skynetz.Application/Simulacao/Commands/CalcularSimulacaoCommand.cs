using System;

namespace Skynetz.Application.Simulacoes.Commands

{
    public class CalcularSimulacaoCommand
    {
        public Guid VisitanteId { get; private set; }
        public string DddOrigem { get; private set; }
        public string DddDestino { get; private set; }
        public int Minutos { get; private set; }
        public string NomePlano { get; private set; }

        public CalcularSimulacaoCommand(
            Guid visitanteId,
            string dddOrigem,
            string dddDestino,
            int minutos,
            string nomePlano)
        {
            VisitanteId = visitanteId;
            DddOrigem = dddOrigem;
            DddDestino = dddDestino;
            Minutos = minutos;
            NomePlano = nomePlano;
        }
    }
}