using DSR.Cats.Server.Entities.Abstract;
using DSR.Cats.Server.Domain.Models;
using DSR.Cats.Server.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DSR.Cats.Server.Services
{
    public class BreedsService : IBreedsService
    {
        private readonly IRepositoryContext _repository;

        public BreedsService(IRepositoryContext repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<Breed>> GetAllAsync()
        {
            return await _repository.Breeds.ToListAsync();
        }

        public async Task<int> CreateAsync(string name)
        {
            Breed breed = new Breed()
            {
                Name = name,
            };
            _repository.Breeds.Add(breed);
            await _repository.SaveChangesAsync();
            return breed.Id;
        }

        public async Task<Breed> FindByNameAsync(string name)
        {
            return await _repository.Breeds.FirstOrDefaultAsync(b => b.Name == name);
        }
    }
}
