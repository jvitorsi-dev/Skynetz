namespace Skynetz.Application.Simulacoes.DTOs
{
    public class PlanoComparadoDto
    {
        public string NomePlano { get; private set; }
        public int MinutosIncluidos { get; private set; }
        public int MinutosExcedentes { get; private set; }
        public decimal ValorComPlano { get; private set; }
        public decimal DiferencaParaSemPlano { get; private set; }

        public PlanoComparadoDto(
            string nomePlano,
            int minutosIncluidos,
            int minutosExcedentes,
            decimal valorComPlano,
            decimal diferencaParaSemPlano)
        {
            NomePlano = nomePlano;
            MinutosIncluidos = minutosIncluidos;
            MinutosExcedentes = minutosExcedentes;
            ValorComPlano = valorComPlano;
            DiferencaParaSemPlano = diferencaParaSemPlano;
        }
    }
}
