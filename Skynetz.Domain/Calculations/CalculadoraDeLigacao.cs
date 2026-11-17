using System;
using Skynetz.Domain.Entities;

namespace Skynetz.Domain.Calculations
{
    public class CalculadoraDeLigacao
    {
        private const decimal AcrescimoExcedente = 1.10m;

        public ResultadoCalculo Calcular(
            Tarifa tarifa,
            Plano plano,
            int minutos)
        {
            if (tarifa == null)
                throw new ArgumentNullException("tarifa");

            if (plano == null)
                throw new ArgumentNullException("plano");

            if (minutos < 0)
                throw new ArgumentOutOfRangeException(
                    "minutos",
                    "O tempo da ligação não pode ser negativo.");

            var minutosExcedentes =
                Math.Max(0, minutos - plano.MinutosIncluidos);

            var valorSemPlano =
                minutos * tarifa.ValorPorMinuto;

            var valorComPlano =
                minutosExcedentes
                * tarifa.ValorPorMinuto
                * AcrescimoExcedente;

            return new ResultadoCalculo(
                valorComPlano,
                valorSemPlano,
                minutosExcedentes);
        }
    }
}