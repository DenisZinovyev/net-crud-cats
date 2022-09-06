using DSR.Cats.Server.Entities.Abstract;
using DSR.Cats.Server.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DSR.Cats.Server.Entities.Extentions;

namespace DSR.Cats.Server.Entities
{
    public class RepositoryContext : DbContext, IRepositoryContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureUser();
            builder.ConfigureCat();
            builder.ConfigureBreed();
        }

        public DbSet<User> UsersDbSet { get; set; }
        public DbSet<Cat> CatsDbSet { get; set; }
        public DbSet<Breed> BreedsDbSet { get; set; }

        public DbSet<Breed> Breeds => Set<Breed>();
        public DbSet<Cat> Cats => Set<Cat>();
        public DbSet<User> Users => Set<User>();

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
