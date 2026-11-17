using Skynetz.Domain.Entities;
using System.Collections.Generic;

namespace Skynetz.Application.Abstractions.Persistence
{
    public interface ITarifaRepository
    {
        Tarifa ObterPorRota(string dddOrigem, string dddDestino);
        IReadOnlyList<Tarifa> ListarTodas();
    }
}