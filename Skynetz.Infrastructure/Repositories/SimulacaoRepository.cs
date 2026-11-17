using Skynetz.Application.Abstractions.Persistence;
using Skynetz.Domain.Entities;
using Skynetz.Infrastructure.Persistence;

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
    }
}