using System;

namespace Skynetz.Domain.Entities
{
    public class Tarifa
    {
        public string DddOrigem { get; private set; }
        public string DddDestino { get; private set; }
        public decimal ValorPorMinuto { get; private set; }

        public Tarifa(string dddOrigem, string dddDestino, decimal valorPorMinuto)
        {
            if (string.IsNullOrWhiteSpace(dddOrigem))
                throw new ArgumentException("O DDD de origem é obrigatório.");

            if (string.IsNullOrWhiteSpace(dddDestino))
                throw new ArgumentException("O DDD de destino é obrigatório.");

            if (valorPorMinuto <= 0)
                throw new ArgumentOutOfRangeException(
                    "valorPorMinuto",
                    "A tarifa deve ser maior que zero.");

            DddOrigem = dddOrigem;
            DddDestino = dddDestino;
            ValorPorMinuto = valorPorMinuto;
        }
    }
}