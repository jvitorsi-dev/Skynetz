using System;

namespace Skynetz.Application.Simulacoes.DTOs
{
    public class SimulacaoHistoricoDto
    {
        public Guid Id { get; private set; }
        public string DddOrigem { get; private set; }
        public string DddDestino { get; private set; }
        public int MinutosLigacao { get; private set; }
        public string NomePlano { get; private set; }
        public int MinutosExcedentes { get; private set; }
        public decimal ValorComPlano { get; private set; }
        public decimal ValorSemPlano { get; private set; }
        public DateTime CriadaEmUtc { get; private set; }

        public SimulacaoHistoricoDto(
            Guid id,
            string dddOrigem,
            string dddDestino,
            int minutosLigacao,
            string nomePlano,
            int minutosExcedentes,
            decimal valorComPlano,
            decimal valorSemPlano,
            DateTime criadaEmUtc)
        {
            Id = id;
            DddOrigem = dddOrigem;
            DddDestino = dddDestino;
            MinutosLigacao = minutosLigacao;
            NomePlano = nomePlano;
            MinutosExcedentes = minutosExcedentes;
            ValorComPlano = valorComPlano;
            ValorSemPlano = valorSemPlano;
            CriadaEmUtc = criadaEmUtc;
        }
    }
}