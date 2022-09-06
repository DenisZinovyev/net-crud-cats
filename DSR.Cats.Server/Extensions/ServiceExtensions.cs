using DSR.Cats.Server.Entities;
using DSR.Cats.Server.Entities.Abstract;
using DSR.Cats.Server.Services;
using DSR.Cats.Server.Services.Abstract;
using DSR.Cats.Server.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace DSR.Cats.Server.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CrudCatsDatabase");
            services.AddDbContext<RepositoryContext>(options => options.UseNpgsql(connectionString));
        }

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryContext, RepositoryContext>();
        }

        public static void ConfigureAuthenificaiton(this IServiceCollection services, AuthConfiguration authConfiguration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = authConfiguration.Issuer,
                        ValidAudience = authConfiguration.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(authConfiguration.IssuerSigningKey),
                    };
                });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning();
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration, string allowSpecificOrigins)
        {
            string[] origins = configuration.GetSection("CorsOrigins").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy(allowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(origins).AllowAnyHeader();
                });
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "ASP.NET Core Web API"
                });
            });
        }

        public static void ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ICatsService, CatsService>();
            services.AddScoped<IBreedsService, BreedsService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddHttpContextAccessor();
        }
    }
}