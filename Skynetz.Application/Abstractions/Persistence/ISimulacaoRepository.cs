using Skynetz.Domain.Entities;

namespace Skynetz.Application.Abstractions.Persistence
{
    public interface ISimulacaoRepository
    {
        void Adicionar(Simulacao simulacao);
    }
}