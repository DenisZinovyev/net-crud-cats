using DSR.Cats.Server.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DSR.Cats.Server.Entities.Extentions
{
    public static class FluentApiConfigurationsExtentions
    {
        public static void ConfigureUser(this ModelBuilder builder)
        {
            var typeBuilder = builder.Entity<User>();
            typeBuilder.HasKey(u => u.Id);
            typeBuilder.HasIndex(u => u.Email).IsUnique();
            typeBuilder.Property(u => u.Email).IsRequired();
            typeBuilder.Property(u => u.FirstName).IsRequired();
            typeBuilder.Property(u => u.LastName).IsRequired();
            typeBuilder.Property(u => u.Password).IsRequired();
        }

        public static void ConfigureCat(this ModelBuilder builder)
        {
            var typeBuilder = builder.Entity<Cat>();
            typeBuilder.HasKey(u => u.Id);
            typeBuilder.Property(u => u.Name).IsRequired();
            typeBuilder.Property(u => u.BreedId).IsRequired();
        }

        public static void ConfigureBreed(this ModelBuilder builder)
        {
            var typeBuilder = builder.Entity<Breed>();
            typeBuilder.HasKey(u => u.Id);
            typeBuilder.Property(u => u.Name).IsRequired();
        }
    }
}
