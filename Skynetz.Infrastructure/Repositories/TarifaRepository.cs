using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Domain.Entities;
using Skynetz.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Skynetz.Infrastructure.Repositories
{
    public class TarifaRepository : ITarifaRepository
    {
        public Tarifa ObterPorRota(string dddOrigem, string dddDestino)
        {
            using (var context = new SkynetzDbContext())
            {
                return context.Tarifas.SingleOrDefault(t =>
                    t.DddOrigem == dddOrigem &&
                    t.DddDestino == dddDestino);
            }
        }

        public IReadOnlyList<Tarifa> ListarTodas()
        {
            using (var context = new SkynetzDbContext())
            {
                return context.Tarifas
                    .OrderBy(t => t.DddOrigem)
                    .ThenBy(t => t.DddDestino)
                    .ToList();
            }
        }
    }
}