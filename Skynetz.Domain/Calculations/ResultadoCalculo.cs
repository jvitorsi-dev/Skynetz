namespace Skynetz.Domain.Calculations
{
    public class ResultadoCalculo
    {
        public decimal ValorComPlano { get; private set; }
        public decimal ValorSemPlano { get; private set; }
        public int MinutosExcedentes { get; private set; }

        public ResultadoCalculo(
            decimal valorComPlano,
            decimal valorSemPlano,
            int minutosExcedentes)
        {
            ValorComPlano = valorComPlano;
            ValorSemPlano = valorSemPlano;
            MinutosExcedentes = minutosExcedentes;
        }
    }
}