using System.Collections.Generic;

namespace Skynetz.Application.Simulacoes.DTOs
{
    public class CompararPlanosResultDto
    {
        public string DddOrigem { get; private set; }
        public string DddDestino { get; private set; }
        public int Minutos { get; private set; }
        public decimal ValorSemPlano { get; private set; }
        public IReadOnlyList<PlanoComparadoDto> Planos { get; private set; }

        public CompararPlanosResultDto(
            string dddOrigem,
            string dddDestino,
            int minutos,
            decimal valorSemPlano,
            IReadOnlyList<PlanoComparadoDto> planos)
        {
            DddOrigem = dddOrigem;
            DddDestino = dddDestino;
            Minutos = minutos;
            ValorSemPlano = valorSemPlano;
            Planos = planos;
        }
    }
}
