using Skynetz.Domain.Entities;
using System.Collections.Generic;

namespace Skynetz.Application.Abstractions.Persistence
{
    public interface IPlanoRepository
    {
        Plano ObterPorNome(string nome);
        IReadOnlyList<Plano> ListarTodos();
    }
}