using Skynetz.Application.Simulacoes.UseCases;
using Skynetz.Domain.Calculations;
using Skynetz.Infrastructure.Repositories;

namespace Skynetz.Web.App_Start
{
    public static class ApplicationComposition
    {
        public static CalcularSimulacaoUseCase CriarCasoDeUsoCalcularSimulacao()
        {
            return new CalcularSimulacaoUseCase(
                new TarifaRepository(),
                new PlanoRepository(),
                new SimulacaoRepository(),
                new CalculadoraDeLigacao());
        }

        public static ObterOpcoesSimulacaoUseCase CriarCasoDeUsoObterOpcoesSimulacao()
        {
            return new ObterOpcoesSimulacaoUseCase(
                new TarifaRepository(),
                new PlanoRepository());
        }

        public static ListarHistoricoSimulacoesUseCase CriarCasoDeUsoListarHistorico()
        {
            return new ListarHistoricoSimulacoesUseCase(
                new SimulacaoRepository());
        }
        public static CompararPlanosUseCase CriarCasoDeUsoCompararPlanos()
        {
            return new CompararPlanosUseCase(
                new TarifaRepository(),
                new PlanoRepository(),
                new CalculadoraDeLigacao());
        }
    }
}