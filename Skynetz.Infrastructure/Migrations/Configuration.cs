namespace Skynetz.Infrastructure.Migrations
{
    using Skynetz.Domain.Entities;
    using Skynetz.Infrastructure.Persistence;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SkynetzDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SkynetzDbContext context)
        {
            context.Tarifas.AddOrUpdate(
                t => new { t.DddOrigem, t.DddDestino },
                new Tarifa("011", "016", 1.90m),
                new Tarifa("016", "011", 2.90m),
                new Tarifa("011", "017", 1.70m),
                new Tarifa("017", "011", 2.70m),
                new Tarifa("011", "018", 0.90m),
                new Tarifa("018", "011", 1.90m)
            );

            context.Planos.AddOrUpdate(
                p => p.Nome,
                new Plano("FaleMais 30", 30),
                new Plano("FaleMais 60", 60),
                new Plano("FaleMais 120", 120)
            );
        }
    }
}
