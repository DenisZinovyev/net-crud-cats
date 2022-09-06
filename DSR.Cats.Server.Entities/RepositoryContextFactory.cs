using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DSR.Cats.Server.Entities
{
    /// <summary>
    /// Uses only for create migrations
    /// </summary>
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        const string DesignTimeConnectionString = @"Host=localhost;Port=5432;Database=crud-cats;Username=postgres;Password=pg";
        public RepositoryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
            optionsBuilder.UseNpgsql(DesignTimeConnectionString);

            return new RepositoryContext(optionsBuilder.Options);
        }
    }
}
