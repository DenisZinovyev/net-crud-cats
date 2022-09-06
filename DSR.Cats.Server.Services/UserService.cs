using DSR.Cats.Server.Entities.Abstract;
using DSR.Cats.Server.Domain.Models;
using DSR.Cats.Server.Services.Abstract;
using DSR.Cats.Server.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DSR.Cats.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryContext _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IRepositoryContext repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CreateAsync(string email, string firstName, string lastName, string password)
        {
            var foundUser = await FindByEmailAsync(email);
            if (foundUser != null)
            {
                throw new AlreadyExistException();
            }

            var user = new User()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password,
            };
            _repository.Users.Add(user);
            await _repository.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User> GetCurrentAsync()
        {
            return await FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public async Task<User> GetAsync(int id)
        {
            var user = await _repository.Users.FindAsync(id);
            if (user == null)
                throw new EntityNotFoundException();

            return user;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await _repository.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
