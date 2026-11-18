using System;
using System.Collections.Generic;
using System.Linq;
using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Application.Simulacoes.DTOs;

namespace Skynetz.Application.Simulacoes.UseCases
{
    public class ListarHistoricoSimulacoesUseCase
    {
        private readonly ISimulacaoRepository _simulacaoRepository;

        public ListarHistoricoSimulacoesUseCase(
            ISimulacaoRepository simulacaoRepository)
        {
            _simulacaoRepository = simulacaoRepository
                ?? throw new ArgumentNullException("simulacaoRepository");
        }

        public IReadOnlyList<SimulacaoHistoricoDto> Executar(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentOutOfRangeException(
                    "quantidade",
                    "A quantidade deve ser maior que zero.");

            return _simulacaoRepository
                .ListarRecentes(quantidade)
                .Select(s => new SimulacaoHistoricoDto(
                    s.Id,
                    s.DddOrigem,
                    s.DddDestino,
                    s.MinutosLigacao,
                    s.NomePlano,
                    s.MinutosExcedentes,
                    s.ValorComPlano,
                    s.ValorSemPlano,
                    s.CriadaEmUtc))
                .ToList();
        }
    }
}