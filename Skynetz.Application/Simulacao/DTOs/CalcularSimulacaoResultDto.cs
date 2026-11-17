using System;

namespace Skynetz.Application.Simulacoes.DTOs

{
    public class CalcularSimulacaoResultDto
    {
        public Guid SimulacaoId { get; private set; }
        public string DddOrigem { get; private set; }
        public string DddDestino { get; private set; }
        public int Minutos { get; private set; }
        public string NomePlano { get; private set; }
        public int MinutosExcedentes { get; private set; }
        public decimal ValorComPlano { get; private set; }
        public decimal ValorSemPlano { get; private set; }

        public CalcularSimulacaoResultDto(
            Guid simulacaoId,
            string dddOrigem,
            string dddDestino,
            int minutos,
            string nomePlano,
            int minutosExcedentes,
            decimal valorComPlano,
            decimal valorSemPlano)
        {
            SimulacaoId = simulacaoId;
            DddOrigem = dddOrigem;
            DddDestino = dddDestino;
            Minutos = minutos;
            NomePlano = nomePlano;
            MinutosExcedentes = minutosExcedentes;
            ValorComPlano = valorComPlano;
            ValorSemPlano = valorSemPlano;
        }
    }
}