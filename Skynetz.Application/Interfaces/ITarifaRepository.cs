using Skynetz.Domain.Entities;

namespace Skynetz.Application.Interfaces
{
    public interface ITarifaRepository
    {
        Tarifa ObterPorRota(string dddOrigem, string dddDestino);
    }
}