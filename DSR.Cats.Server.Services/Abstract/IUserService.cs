using DSR.Cats.Server.Domain.Models;
using System.Threading.Tasks;

namespace DSR.Cats.Server.Services.Abstract
{
    public interface IUserService
    {
        Task<int> CreateAsync(string email, string firstName, string lastName, string password);
        Task<User> GetCurrentAsync();
        Task<User> GetAsync(int id);
        Task<User> FindByEmailAsync(string email);
    }
}
