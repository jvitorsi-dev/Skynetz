namespace Skynetz.Application.Simulacoes.DTOs
{
    public class PlanoOpcaoDto
    {
        public string Nome { get; private set; }
        public int MinutosIncluidos { get; private set; }

        public PlanoOpcaoDto(string nome, int minutosIncluidos)
        {
            Nome = nome;
            MinutosIncluidos = minutosIncluidos;
        }
    }
}