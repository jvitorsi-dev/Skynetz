using System;
using System.Collections.Generic;
using NUnit.Framework;
using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Application.Simulacoes.UseCases;
using Skynetz.Domain.Calculations;
using Skynetz.Domain.Entities;

namespace Skynetz.Tests.Application
{
    [TestFixture]
    public class ListarHistoricoSimulacoesUseCaseTests
    {
        [Test]
        public void Executar_PassaVisitanteAoRepositorioEMapeiaResultado()
        {
            var visitanteId = Guid.NewGuid();
            var tarifa = new Tarifa("011", "016", 1.90m);
            var plano = new Plano("FaleMais 30", 30);
            var calculo = new CalculadoraDeLigacao()
                .Calcular(tarifa, plano, 40);

            var simulacao = new Simulacao(
                visitanteId,
                tarifa,
                plano,
                40,
                calculo);

            var repositorio = new FakeSimulacaoRepository
            {
                Simulacoes = new List<Simulacao> { simulacao }
            };

            var useCase = new ListarHistoricoSimulacoesUseCase(repositorio);

            var resultado = useCase.Executar(visitanteId, 20);

            Assert.That(repositorio.FoiConsultado, Is.True);
            Assert.That(repositorio.VisitanteRecebido, Is.EqualTo(visitanteId));
            Assert.That(repositorio.QuantidadeRecebida, Is.EqualTo(20));
            Assert.That(resultado, Has.Count.EqualTo(1));
            Assert.That(resultado[0].Id, Is.EqualTo(simulacao.Id));
            Assert.That(resultado[0].ValorComPlano, Is.EqualTo(20.90m));
        }

        [Test]
        public void Executar_ComGuidVazio_RejeitaAntesDeConsultarRepositorio()
        {
            var repositorio = new FakeSimulacaoRepository();
            var useCase = new ListarHistoricoSimulacoesUseCase(repositorio);

            Assert.Throws<ArgumentException>(
                () => useCase.Executar(Guid.Empty, 20));

            Assert.That(repositorio.FoiConsultado, Is.False);
        }

        private sealed class FakeSimulacaoRepository : ISimulacaoRepository
        {
            public IReadOnlyList<Simulacao> Simulacoes { get; set; }
            public Guid VisitanteRecebido { get; private set; }
            public int QuantidadeRecebida { get; private set; }
            public bool FoiConsultado { get; private set; }

            public void Adicionar(Simulacao simulacao)
            {
            }

            public IReadOnlyList<Simulacao> ListarRecentes(
                Guid visitanteId,
                int quantidade)
            {
                FoiConsultado = true;
                VisitanteRecebido = visitanteId;
                QuantidadeRecebida = quantidade;

                return Simulacoes ?? new List<Simulacao>();
            }
        }
    }
}