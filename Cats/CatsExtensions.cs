namespace Microsoft.Extensions.DependencyInjection
{
    using DSR.CrudCats.Cats;

    public static class CatsExtensions
    {
        public static void AddCats(this IServiceCollection services)
        {
            services.AddScoped<ICatsRepository, CatsRepository>();
            services.AddScoped<ICatsService, CatsService>();
        }
    }
}