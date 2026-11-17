using Skynetz.Domain.Calculations;
using Skynetz.Domain.Entities;
using System;

namespace Skynetz.Domain.Entities
{
    public class Simulacao
    {
        public Guid Id { get; private set; }
        public string DddOrigem { get; private set; }
        public string DddDestino { get; private set; }
        public int MinutosLigacao { get; private set; }

        public string NomePlano { get; private set; }
        public int MinutosIncluidos { get; private set; }
        public decimal TarifaPorMinuto { get; private set; }

        public int MinutosExcedentes { get; private set; }
        public decimal ValorComPlano { get; private set; }
        public decimal ValorSemPlano { get; private set; }
        public DateTime CriadaEmUtc { get; private set; }

        private Simulacao()
        {
            // Construtor sem parâmetros para facilitar a materialização pelo EF6.
        }

        public Simulacao(
            Tarifa tarifa,
            Plano plano,
            int minutosLigacao,
            ResultadoCalculo resultado)
        {
            if (tarifa == null)
                throw new ArgumentNullException("tarifa");

            if (plano == null)
                throw new ArgumentNullException("plano");

            if (resultado == null)
                throw new ArgumentNullException("resultado");

            if (minutosLigacao < 0)
                throw new ArgumentOutOfRangeException(
                    "minutosLigacao",
                    "O tempo da ligação não pode ser negativo.");

            Id = Guid.NewGuid();
            DddOrigem = tarifa.DddOrigem;
            DddDestino = tarifa.DddDestino;
            MinutosLigacao = minutosLigacao;

            NomePlano = plano.Nome;
            MinutosIncluidos = plano.MinutosIncluidos;
            TarifaPorMinuto = tarifa.ValorPorMinuto;

            MinutosExcedentes = resultado.MinutosExcedentes;
            ValorComPlano = resultado.ValorComPlano;
            ValorSemPlano = resultado.ValorSemPlano;
            CriadaEmUtc = DateTime.UtcNow;
        }
    }
}