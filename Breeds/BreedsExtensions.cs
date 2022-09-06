namespace Microsoft.Extensions.DependencyInjection
{
    using DSR.CrudCats.Breeds;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    public static class BreedsExtensions
    {
        public static void AddBreeds(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BreedsConfiguration>(configuration.GetSection("Breeds"));
            services.AddScoped<IBreedsRepository, BreedsRepository>();
            services.AddScoped<IBreedsService, BreedsService>();
            
            // To access methods with package visibility for configuration
            services.AddScoped<BreedsService, BreedsService>(); 

            var breedsConfiguration = services
                .BuildServiceProvider()
                .GetService<IOptions<BreedsConfiguration>>()
                .Value;

            // We use scope-managed DbContext and need a new scope to commit 
            // transaction in BreedsService::Initialize
            var scopeFactory = services
                    .BuildServiceProvider()
                    .GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                scope
                    .ServiceProvider
                    .GetService<BreedsService>()
                    .Initialize(breedsConfiguration);
            }
        }
    }
}