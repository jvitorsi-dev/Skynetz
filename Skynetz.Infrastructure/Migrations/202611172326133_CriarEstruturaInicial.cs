namespace Skynetz.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriarEstruturaInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Planos",
                c => new
                    {
                        Nome = c.String(nullable: false, maxLength: 50),
                        MinutosIncluidos = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Nome);
            
            CreateTable(
                "dbo.Simulacoes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DddOrigem = c.String(nullable: false, maxLength: 3),
                        DddDestino = c.String(nullable: false, maxLength: 3),
                        MinutosLigacao = c.Int(nullable: false),
                        NomePlano = c.String(nullable: false, maxLength: 50),
                        MinutosIncluidos = c.Int(nullable: false),
                        TarifaPorMinuto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinutosExcedentes = c.Int(nullable: false),
                        ValorComPlano = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorSemPlano = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CriadaEmUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tarifas",
                c => new
                    {
                        DddOrigem = c.String(nullable: false, maxLength: 3),
                        DddDestino = c.String(nullable: false, maxLength: 3),
                        ValorPorMinuto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.DddOrigem, t.DddDestino });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tarifas");
            DropTable("dbo.Simulacoes");
            DropTable("dbo.Planos");
        }
    }
}
