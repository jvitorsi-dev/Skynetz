using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Domain.Entities;
using Skynetz.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Skynetz.Infrastructure.Repositories
{
    public class PlanoRepository : IPlanoRepository
    {
        public Plano ObterPorNome(string nome)
        {
            using (var context = new SkynetzDbContext())
            {
                return context.Planos.SingleOrDefault(p => p.Nome == nome);
            }
        }

        public IReadOnlyList<Plano> ListarTodos()
        {
            using (var context = new SkynetzDbContext())
            {
                return context.Planos
                    .OrderBy(p => p.MinutosIncluidos)
                    .ToList();
            }
        }
    }
}