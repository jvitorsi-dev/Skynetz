namespace Skynetz.Application.Simulacoes.Queries
{
    public class CompararPlanosQuery
    {
        public string DddOrigem { get; private set; }
        public string DddDestino { get; private set; }
        public int Minutos { get; private set; }

        public CompararPlanosQuery(
            string dddOrigem,
            string dddDestino,
            int minutos)
        {
            DddOrigem = dddOrigem;
            DddDestino = dddDestino;
            Minutos = minutos;
        }
    }
}
