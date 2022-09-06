using DSR.Cats.Server.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DSR.Cats.Server.Services.Abstract
{
    public interface IBreedsService
    {
        Task<int> CreateAsync(string name);
        Task<IReadOnlyCollection<Breed>> GetAllAsync();
        Task<Breed> FindByNameAsync(string name);
    }
}
