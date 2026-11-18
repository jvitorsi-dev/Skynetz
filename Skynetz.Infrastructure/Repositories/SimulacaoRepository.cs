using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Domain.Entities;
using Skynetz.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Skynetz.Infrastructure.Repositories
{
    public class SimulacaoRepository : ISimulacaoRepository
    {
        public void Adicionar(Simulacao simulacao)
        {
            using (var context = new SkynetzDbContext())
            {
                context.Simulacoes.Add(simulacao);
                context.SaveChanges();
            }
        }

        public IReadOnlyList<Simulacao> ListarRecentes(int quantidade)
        {
            using (var context = new SkynetzDbContext())
            {
                return context.Simulacoes
                    .OrderByDescending(s => s.CriadaEmUtc)
                    .Take(quantidade)
                    .ToList();
            }
        }
    }
}