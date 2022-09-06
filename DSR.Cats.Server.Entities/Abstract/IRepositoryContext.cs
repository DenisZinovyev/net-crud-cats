using DSR.Cats.Server.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DSR.Cats.Server.Entities.Abstract
{
    public interface IRepositoryContext
    {
        DbSet<Breed> Breeds { get; }
        DbSet<Cat> Cats { get; }
        DbSet<User> Users { get; }
        Task<int> SaveChangesAsync();
    }
}
