using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Application.Simulacoes.Queries;
using Skynetz.Application.Simulacoes.UseCases;
using Skynetz.Domain.Entities;

namespace Skynetz.Tests.Application
{
    [TestFixture]
    public class CompararPlanosUseCaseTests
    {
        [Test]
        public void Executar_ComTresPlanos_RetornaComparativoSemSalvarSimulacoes()
        {
            var repositorio = new FakeCatalogoRepository
            {
                TarifaParaRetornar = new Tarifa("011", "016", 1.90m),
                Planos = new List<Plano>
                {
                    new Plano("FaleMais 30", 30),
                    new Plano("FaleMais 60", 60),
                    new Plano("FaleMais 120", 120)
                }
            };

            var useCase = new CompararPlanosUseCase(
                repositorio,
                repositorio,
                new Skynetz.Domain.Calculations.CalculadoraDeLigacao());

            var resultado = useCase.Executar(
                new CompararPlanosQuery("011", "016", 80));

            Assert.That(resultado.ValorSemPlano, Is.EqualTo(152.00m));
            Assert.That(resultado.Planos, Has.Count.EqualTo(3));

            var faleMais30 = resultado.Planos
                .Single(p => p.NomePlano == "FaleMais 30");
            var faleMais60 = resultado.Planos
                .Single(p => p.NomePlano == "FaleMais 60");
            var faleMais120 = resultado.Planos
                .Single(p => p.NomePlano == "FaleMais 120");

            Assert.That(faleMais30.ValorComPlano, Is.EqualTo(104.50m));
            Assert.That(faleMais30.DiferencaParaSemPlano, Is.EqualTo(47.50m));

            Assert.That(faleMais60.ValorComPlano, Is.EqualTo(41.80m));
            Assert.That(faleMais60.DiferencaParaSemPlano, Is.EqualTo(110.20m));

            Assert.That(faleMais120.ValorComPlano, Is.EqualTo(0.00m));
            Assert.That(faleMais120.DiferencaParaSemPlano, Is.EqualTo(152.00m));
        }

        [Test]
        public void Executar_SemTarifa_LancaExcecao()
        {
            var repositorio = new FakeCatalogoRepository
            {
                TarifaParaRetornar = null,
                Planos = new List<Plano>
                {
                    new Plano("FaleMais 30", 30)
                }
            };

            var useCase = new CompararPlanosUseCase(
                repositorio,
                repositorio,
                new Skynetz.Domain.Calculations.CalculadoraDeLigacao());

            Assert.Throws<InvalidOperationException>(
                () => useCase.Executar(
                    new CompararPlanosQuery("018", "017", 100)));
        }

        private sealed class FakeCatalogoRepository :
            ITarifaRepository,
            IPlanoRepository
        {
            public Tarifa TarifaParaRetornar { get; set; }
            public IReadOnlyList<Plano> Planos { get; set; }

            public Tarifa ObterPorRota(string dddOrigem, string dddDestino)
            {
                return TarifaParaRetornar;
            }

            public IReadOnlyList<Tarifa> ListarTodas()
            {
                return new List<Tarifa>();
            }

            public Plano ObterPorNome(string nome)
            {
                return Planos.FirstOrDefault(p => p.Nome == nome);
            }

            public IReadOnlyList<Plano> ListarTodos()
            {
                return Planos;
            }
        }
    }
}