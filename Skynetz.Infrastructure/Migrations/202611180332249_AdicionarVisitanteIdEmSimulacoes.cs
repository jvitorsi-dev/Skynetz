namespace Skynetz.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AdicionarVisitanteIdEmSimulacoes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Simulacoes", "VisitanteId", c => c.Guid(nullable: false));

            // EF/SQL Server pode criar um default Guid.Empty para preencher
            // linhas antigas ao adicionar uma coluna obrigatória. Removemos
            // esse default depois da alteração: a aplicação deve sempre
            // fornecer o identificador real do visitante.
            Sql(@"
DECLARE @defaultConstraint sysname;
DECLARE @dropConstraintSql nvarchar(4000);

SELECT @defaultConstraint = dc.name
FROM sys.default_constraints AS dc
INNER JOIN sys.columns AS c
    ON c.object_id = dc.parent_object_id
    AND c.column_id = dc.parent_column_id
WHERE dc.parent_object_id = OBJECT_ID(N'dbo.Simulacoes')
    AND c.name = N'VisitanteId';

IF @defaultConstraint IS NOT NULL
BEGIN
    SET @dropConstraintSql = N'ALTER TABLE dbo.Simulacoes DROP CONSTRAINT '
        + QUOTENAME(@defaultConstraint);
    EXEC sp_executesql @dropConstraintSql;
END
");

            CreateIndex(
                "dbo.Simulacoes",
                "VisitanteId",
                name: "IX_Simulacoes_VisitanteId");
        }

        public override void Down()
        {
            DropIndex("dbo.Simulacoes", "IX_Simulacoes_VisitanteId");
            DropColumn("dbo.Simulacoes", "VisitanteId");
        }
    }
}
