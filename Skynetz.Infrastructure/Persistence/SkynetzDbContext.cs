using System.Data.Entity;
using Skynetz.Domain.Entities;

namespace Skynetz.Infrastructure.Persistence
{
    public class SkynetzDbContext : DbContext
    {
        public SkynetzDbContext()
            : base("name=SkynetzConnection")
        {
        }

        public DbSet<Tarifa> Tarifas { get; set; }
        public DbSet<Plano> Planos { get; set; }
        public DbSet<Simulacao> Simulacoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tarifa>()
                .ToTable("Tarifas")
                .HasKey(t => new { t.DddOrigem, t.DddDestino });

            modelBuilder.Entity<Tarifa>()
                .Property(t => t.DddOrigem)
                .IsRequired()
                .HasMaxLength(3);

            modelBuilder.Entity<Tarifa>()
                .Property(t => t.DddDestino)
                .IsRequired()
                .HasMaxLength(3);

            modelBuilder.Entity<Tarifa>()
                .Property(t => t.ValorPorMinuto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Plano>()
                .ToTable("Planos")
                .HasKey(p => p.Nome);

            modelBuilder.Entity<Plano>()
                .Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Simulacao>()
                .ToTable("Simulacoes")
                .HasKey(s => s.Id);

            modelBuilder.Entity<Simulacao>()
                .Property(s => s.DddOrigem)
                .IsRequired()
                .HasMaxLength(3);

            modelBuilder.Entity<Simulacao>()
                .Property(s => s.DddDestino)
                .IsRequired()
                .HasMaxLength(3);

            modelBuilder.Entity<Simulacao>()
                .Property(s => s.NomePlano)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Simulacao>()
                .Property(s => s.TarifaPorMinuto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Simulacao>()
                .Property(s => s.ValorComPlano)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Simulacao>()
                .Property(s => s.ValorSemPlano)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Simulacao>()
                .Property(s => s.CriadaEmUtc)
                .HasColumnType("datetime2");
        }
    }
}