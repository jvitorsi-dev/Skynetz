using Skynetz.Domain.Entities;

namespace Skynetz.Application.Interfaces
{
    public interface IPlanoRepository
    {
        Plano ObterPorNome(string nome);
    }
}