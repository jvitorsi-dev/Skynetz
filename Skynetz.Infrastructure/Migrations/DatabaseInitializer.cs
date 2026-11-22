using System.Data.Entity;
using Skynetz.Infrastructure.Persistence;

namespace Skynetz.Infrastructure.Migrations
{    
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<SkynetzDbContext, Configuration>());
        }
    }
}