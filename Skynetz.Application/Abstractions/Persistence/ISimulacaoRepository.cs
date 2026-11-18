using Skynetz.Domain.Entities;
using System.Collections.Generic;

namespace Skynetz.Application.Abstractions.Persistence
{
    public interface ISimulacaoRepository
    {
        void Adicionar(Simulacao simulacao);
        IReadOnlyList<Simulacao> ListarRecentes(int quantidade);

    }
}