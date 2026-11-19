using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Application.Simulacoes.Commands;
using Skynetz.Application.Simulacoes.UseCases;
using Skynetz.Domain.Calculations;
using Skynetz.Domain.Entities;

namespace Skynetz.Tests.Application
{
    [TestFixture]
    public class CalcularSimulacaoUseCaseTests
    {
        [Test]
        public void Executar_ComDadosValidos_SalvaSimulacaoComVisitante()
        {
            // Arrange
            var visitanteId = Guid.NewGuid();

            var repositorios = new FakeRepositories
            {
                TarifaParaRetornar = new Tarifa("011", "016", 1.90m),
                PlanoParaRetornar = new Plano("FaleMais 30", 30)
            };

            var useCase = new CalcularSimulacaoUseCase(
                repositorios,
                repositorios,
                repositorios,
                new CalculadoraDeLigacao());

            var command = new CalcularSimulacaoCommand(
                visitanteId,
                "011",
                "016",
                40,
                "FaleMais 30");

            // Act
            var resultado = useCase.Executar(command);

            // Assert
            Assert.That(repositorios.QuantidadeSalva, Is.EqualTo(1));
            Assert.That(repositorios.UltimaSimulacao, Is.Not.Null);
            Assert.That(
                repositorios.UltimaSimulacao.VisitanteId,
                Is.EqualTo(visitanteId));

            Assert.That(resultado.MinutosExcedentes, Is.EqualTo(10));
            Assert.That(resultado.ValorComPlano, Is.EqualTo(20.90m));
            Assert.That(resultado.ValorSemPlano, Is.EqualTo(76.00m));
        }

        [Test]
        public void Executar_SemTarifa_NaoSalvaSimulacao()
        {
            // Arrange
            var repositorios = new FakeRepositories
            {
                TarifaParaRetornar = null,
                PlanoParaRetornar = new Plano("FaleMais 30", 30)
            };

            var useCase = new CalcularSimulacaoUseCase(
                repositorios,
                repositorios,
                repositorios,
                new CalculadoraDeLigacao());

            var command = new CalcularSimulacaoCommand(
                Guid.NewGuid(),
                "018",
                "017",
                100,
                "FaleMais 30");

            // Act / Assert
            Assert.Throws<InvalidOperationException>(
                () => useCase.Executar(command));

            Assert.That(repositorios.QuantidadeSalva, Is.EqualTo(0));
        }

        private sealed class FakeRepositories :
            ITarifaRepository,
            IPlanoRepository,
            ISimulacaoRepository
        {
            public Tarifa TarifaParaRetornar { get; set; }
            public Plano PlanoParaRetornar { get; set; }
            public Simulacao UltimaSimulacao { get; private set; }
            public int QuantidadeSalva { get; private set; }

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
                return PlanoParaRetornar;
            }

            public IReadOnlyList<Plano> ListarTodos()
            {
                return new List<Plano>();
            }

            public void Adicionar(Simulacao simulacao)
            {
                UltimaSimulacao = simulacao;
                QuantidadeSalva++;
            }

            public IReadOnlyList<Simulacao> ListarRecentes(
                Guid visitanteId,
                int quantidade)
            {
                return new List<Simulacao>();
            }
        }
    }
}