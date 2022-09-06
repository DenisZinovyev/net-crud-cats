using DSR.Cats.Server.Entities.Abstract;
using DSR.Cats.Server.Domain.Models;
using DSR.Cats.Server.Services.Abstract;
using DSR.Cats.Server.Services.Exceptions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DSR.Cats.Server.Services
{
    public class CatsService : ICatsService
    {
        private readonly IRepositoryContext _repository;
        private readonly IUserService _userService;

        public CatsService(IRepositoryContext repository, IUserService userService)
        {
            _repository = repository;
            _userService = userService;
        }

        public async Task<Cat> GetAsync(int id)
        {
            Cat cat = await _repository.Cats.FindAsync(id);
            if (cat == null)
                throw new EntityNotFoundException();

            return cat;
        }

        public async Task<IReadOnlyCollection<Cat>> GetAllAsync()
        {
            return await _repository.Cats.ToListAsync();
        }

        public async Task<int> CreateAsync(string name, int breedId)
        {
            User currentUser = await _userService.GetCurrentAsync();
            Cat cat = new Cat()
            {
                Name = name,
                BreedId = breedId,
                OwnerId = currentUser.Id,
            };
            _repository.Cats.Add(cat);
            await _repository.SaveChangesAsync();
            return cat.Id;
        }

        public async Task UpdateAsync(int id, string name, int breedId)
        {
            User currentUser = await _userService.GetCurrentAsync();
            var cat = await GetAsync(id);

            if (cat.OwnerId != currentUser.Id)
                throw new AccessDeniedException();

            cat.Name = name;
            cat.BreedId = breedId;
            _repository.Cats.Update(cat);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            User currentUser = await _userService.GetCurrentAsync();
            Cat cat = await GetAsync(id);

            if (cat.OwnerId != currentUser.Id)
                throw new AccessDeniedException();

            _repository.Cats.Remove(cat); // TODO: Remove async
            await _repository.SaveChangesAsync();
        }
    }
}
