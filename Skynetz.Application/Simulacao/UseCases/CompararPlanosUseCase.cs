using System;
using System.Linq;
using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Application.Simulacoes.DTOs;
using Skynetz.Application.Simulacoes.Queries;
using Skynetz.Domain.Calculations;

namespace Skynetz.Application.Simulacoes.UseCases
{
    public class CompararPlanosUseCase
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly IPlanoRepository _planoRepository;
        private readonly CalculadoraDeLigacao _calculadora;

        public CompararPlanosUseCase(
            ITarifaRepository tarifaRepository,
            IPlanoRepository planoRepository,
            CalculadoraDeLigacao calculadora)
        {
            _tarifaRepository = tarifaRepository
                ?? throw new ArgumentNullException("tarifaRepository");
            _planoRepository = planoRepository
                ?? throw new ArgumentNullException("planoRepository");
            _calculadora = calculadora
                ?? throw new ArgumentNullException("calculadora");
        }

        public CompararPlanosResultDto Executar(CompararPlanosQuery query)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            if (query.Minutos < 0)
                throw new ArgumentOutOfRangeException(
                    "query.Minutos",
                    "O tempo da ligação não pode ser negativo.");

            var tarifa = _tarifaRepository.ObterPorRota(
                query.DddOrigem,
                query.DddDestino);

            if (tarifa == null)
                throw new InvalidOperationException(
                    "Não existe tarifa cadastrada para essa rota.");

            var planos = _planoRepository.ListarTodos();

            if (planos == null || planos.Count == 0)
                throw new InvalidOperationException(
                    "Não há planos cadastrados para comparar.");

            var calculos = planos
                .Select(plano => new
                {
                    Plano = plano,
                    Resultado = _calculadora.Calcular(
                        tarifa,
                        plano,
                        query.Minutos)
                })
                .ToList();

            var valorSemPlano = calculos[0].Resultado.ValorSemPlano;

            var planosComparados = calculos
                .Select(item => new PlanoComparadoDto(
                    item.Plano.Nome,
                    item.Plano.MinutosIncluidos,
                    item.Resultado.MinutosExcedentes,
                    item.Resultado.ValorComPlano,
                    valorSemPlano - item.Resultado.ValorComPlano))
                .ToList();

            return new CompararPlanosResultDto(
                query.DddOrigem,
                query.DddDestino,
                query.Minutos,
                valorSemPlano,
                planosComparados);
        }
    }
}
