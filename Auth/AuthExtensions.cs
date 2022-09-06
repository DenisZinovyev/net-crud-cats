namespace Microsoft.Extensions.DependencyInjection
{
    using System.Text;
    using DSR.CrudCats.Auth;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public static class AuthExtensions
    {
        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthConfiguration>(configuration.GetSection("Authentication"));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IIdentityHelper, IdentityHelper>();

            var authConfiguration = services
                .BuildServiceProvider()
                .GetService<IOptions<AuthConfiguration>>()
                .Value;

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = authConfiguration.JwtIssuer,
                        ValidAudience = authConfiguration.JwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.JwtIssuerSigningKey))
                    };
                });
        }
    }
}