using DSR.Cats.Server.Services.Abstract;
using DSR.Cats.Server.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DSR.Cats.Server.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        readonly string AllowSpecificOrigins = "_catsAllowSpecificOrigins";

        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.ConfigureDbContext(Configuration);
            services.ConfigureRepository();
            services.ConfigureDomainServices();
            services.ConfigureVersioning();
            services.ConfigureCors(Configuration, AllowSpecificOrigins);
            services.ConfigureSwagger();

            var serviceProvider = services.BuildServiceProvider();
            var authService = serviceProvider.GetService<IAuthService>();
            services.ConfigureAuthenificaiton(authService.GetAuthConfiguration());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.ConfigureExceptionHandler(Logger);
            app.UseAuthentication();
            app.UseCors(AllowSpecificOrigins);

            app.UseHttpsRedirection();
            app.UseMvc();

            app.ConfigureSwagger();
        }
    }
}
