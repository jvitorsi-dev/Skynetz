using System.Linq;
using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Application.Simulacoes.DTOs;

namespace Skynetz.Application.Simulacoes.UseCases
{
    public class ObterOpcoesSimulacaoUseCase
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly IPlanoRepository _planoRepository;

        public ObterOpcoesSimulacaoUseCase(
            ITarifaRepository tarifaRepository,
            IPlanoRepository planoRepository)
        {
            _tarifaRepository = tarifaRepository;
            _planoRepository = planoRepository;
        }

        public OpcoesSimulacaoDto Executar()
        {
            var tarifas = _tarifaRepository.ListarTodas();
            var planos = _planoRepository.ListarTodos();

            var dddsOrigem = tarifas
                .Select(t => t.DddOrigem)
                .Distinct()
                .OrderBy(ddd => ddd)
                .ToList();

            var dddsDestino = tarifas
                .Select(t => t.DddDestino)
                .Distinct()
                .OrderBy(ddd => ddd)
                .ToList();

            var planosDto = planos
                .Select(p => new PlanoOpcaoDto(
                    p.Nome,
                    p.MinutosIncluidos))
                .ToList();

            return new OpcoesSimulacaoDto(
                dddsOrigem,
                dddsDestino,
                planosDto);
        }
    }
}