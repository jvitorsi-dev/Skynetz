using System.Collections.Generic;

namespace Skynetz.Application.Simulacoes.DTOs
{
    public class OpcoesSimulacaoDto
    {
        public IReadOnlyList<string> DddsOrigem { get; private set; }
        public IReadOnlyList<string> DddsDestino { get; private set; }
        public IReadOnlyList<PlanoOpcaoDto> Planos { get; private set; }

        public OpcoesSimulacaoDto(
            IReadOnlyList<string> dddsOrigem,
            IReadOnlyList<string> dddsDestino,
            IReadOnlyList<PlanoOpcaoDto> planos)
        {
            DddsOrigem = dddsOrigem;
            DddsDestino = dddsDestino;
            Planos = planos;
        }
    }
}