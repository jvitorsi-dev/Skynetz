using Skynetz.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Skynetz.Application.Abstractions.Persistence
{
    public interface ISimulacaoRepository
    {
        void Adicionar(Simulacao simulacao); 
        IReadOnlyList<Simulacao> ListarRecentes(Guid visitanteId, int quantidade);
    }
}