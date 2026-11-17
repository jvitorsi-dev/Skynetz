using System;
using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Application.Simulacoes.Commands;
using Skynetz.Application.Simulacoes.DTOs;
using Skynetz.Domain.Calculations;
using Skynetz.Domain.Entities;

namespace Skynetz.Application.Simulacoes.UseCases
{
    public class CalcularSimulacaoUseCase
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly IPlanoRepository _planoRepository;
        private readonly ISimulacaoRepository _simulacaoRepository;
        private readonly CalculadoraDeLigacao _calculadora;

        public CalcularSimulacaoUseCase(
            ITarifaRepository tarifaRepository,
            IPlanoRepository planoRepository,
            ISimulacaoRepository simulacaoRepository,
            CalculadoraDeLigacao calculadora)
        {
            _tarifaRepository = tarifaRepository
                ?? throw new ArgumentNullException("tarifaRepository");
            _planoRepository = planoRepository
                ?? throw new ArgumentNullException("planoRepository");
            _simulacaoRepository = simulacaoRepository
                ?? throw new ArgumentNullException("simulacaoRepository");
            _calculadora = calculadora
                ?? throw new ArgumentNullException("calculadora");
        }

        public CalcularSimulacaoResultDto Executar(
            CalcularSimulacaoCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            var tarifa = _tarifaRepository.ObterPorRota(
                command.DddOrigem,
                command.DddDestino);

            if (tarifa == null)
                throw new InvalidOperationException(
                    "Não existe tarifa cadastrada para essa rota.");

            var plano = _planoRepository.ObterPorNome(command.NomePlano);

            if (plano == null)
                throw new InvalidOperationException(
                    "O plano informado não foi encontrado.");

            var calculo = _calculadora.Calcular(
                tarifa,
                plano,
                command.Minutos);

            var simulacao = new Simulacao(
                tarifa,
                plano,
                command.Minutos,
                calculo);

            _simulacaoRepository.Adicionar(simulacao);

            return new CalcularSimulacaoResultDto(
                simulacao.Id,
                simulacao.DddOrigem,
                simulacao.DddDestino,
                simulacao.MinutosLigacao,
                simulacao.NomePlano,
                simulacao.MinutosExcedentes,
                simulacao.ValorComPlano,
                simulacao.ValorSemPlano);
        }
    }
}