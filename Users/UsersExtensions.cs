namespace Microsoft.Extensions.DependencyInjection
{
    using DSR.CrudCats.Users;

    public static class UsersExtensions
    {
        public static void AddUsers(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersService, UsersService>();
        }
    }
}