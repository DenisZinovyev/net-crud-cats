using DSR.Cats.Server.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DSR.Cats.Server.Services.Abstract
{
    public interface ICatsService
    {
        Task<IReadOnlyCollection<Cat>> GetAllAsync();
        Task<int> CreateAsync(string name, int breedId);
        Task<Cat> GetAsync(int id);
        Task UpdateAsync(int id, string name, int breedId);
        Task DeleteAsync(int id);
    }
}
