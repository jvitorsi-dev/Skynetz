using System;

namespace Skynetz.Domain.Entities
{
    public class Plano
    {
        public string Nome { get; private set; }
        public int MinutosIncluidos { get; private set; }

        private Plano()
        {
            // Usado pelo EF6 ao carregar a entidade do banco.
        }

        public Plano(string nome, int minutosIncluidos)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do plano é obrigatório.");

            if (minutosIncluidos < 0)
                throw new ArgumentOutOfRangeException(
                    "minutosIncluidos",
                    "Os minutos incluídos não podem ser negativos.");

            Nome = nome;
            MinutosIncluidos = minutosIncluidos;
        }
    }
}