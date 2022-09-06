namespace Microsoft.Extensions.DependencyInjection
{
    using DSR.CrudCats.Persistence;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public static class PersistenceExtensions
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: FIXME: Proper configuration
            var connectionString = configuration.GetConnectionString("CrudCatsDatabase");
            services.AddDbContext<PersistenceContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IPersistenceContext, PersistenceContext>();

            // We use scope-managed DbContext and need a new scope to commit 
            // transaction in Database::Migrate
            var scopeFactory = services
                    .BuildServiceProvider()
                    .GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                scope
                    .ServiceProvider
                    .GetService<PersistenceContext>()
                    .Database
                    .Migrate();
            }
            
            services.AddSingleton<IPersistenceContextFactory, PersistenceContextFactory>();
        }
    }
}