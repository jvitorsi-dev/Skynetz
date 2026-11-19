using NUnit.Framework;
using Skynetz.Domain.Calculations;
using Skynetz.Domain.Entities;

namespace Skynetz.Tests.Domain
{
    [TestFixture]
    public class CalculadoraDeLigacaoTests
    {
        [Test]
        public void Calcular_DeveCobrarApenasMinutosExcedentesComAcrescimo()
        {
            // Arrange
            var tarifa = new Tarifa("011", "016", 1.90m);
            var plano = new Plano("FaleMais 30", 30);
            var calculadora = new CalculadoraDeLigacao();

            // Act: 40 minutos, franquia de 30; 10 minutos excedentes.
            var resultado = calculadora.Calcular(tarifa, plano, 40);

            // Assert
            Assert.That(resultado.MinutosExcedentes, Is.EqualTo(10));
            Assert.That(resultado.ValorComPlano, Is.EqualTo(20.90m));
            Assert.That(resultado.ValorSemPlano, Is.EqualTo(76.00m));
        }

        [Test]
        public void Calcular_AbaixoDaFranquia_NaoCobraComPlano()
        {
            var tarifa = new Tarifa("011", "016", 1.90m);
            var plano = new Plano("FaleMais 30", 30);
            var calculadora = new CalculadoraDeLigacao();

            var resultado = calculadora.Calcular(tarifa, plano, 20);

            Assert.That(resultado.MinutosExcedentes, Is.EqualTo(0));
            Assert.That(resultado.ValorComPlano, Is.EqualTo(0.00m));
            Assert.That(resultado.ValorSemPlano, Is.EqualTo(38.00m));
        }

        [Test]
        public void Calcular_IgualAFranquia_NaoCobraExcedentes()
        {
            var tarifa = new Tarifa("011", "016", 1.90m);
            var plano = new Plano("FaleMais 30", 30);
            var calculadora = new CalculadoraDeLigacao();

            var resultado = calculadora.Calcular(tarifa, plano, 30);

            Assert.That(resultado.MinutosExcedentes, Is.EqualTo(0));
            Assert.That(resultado.ValorComPlano, Is.EqualTo(0.00m));
            Assert.That(resultado.ValorSemPlano, Is.EqualTo(57.00m));
        }

        [Test]
        public void Calcular_MinutosNegativos_LancaExcecao()
        {
            var tarifa = new Tarifa("011", "016", 1.90m);
            var plano = new Plano("FaleMais 30", 30);
            var calculadora = new CalculadoraDeLigacao();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => calculadora.Calcular(tarifa, plano, -1));
        }
    }
}