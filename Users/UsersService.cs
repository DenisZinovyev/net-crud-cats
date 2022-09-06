namespace DSR.CrudCats.Users
{
    using DSR.CrudCats.Auth;
    using DSR.CrudCats.Common;
    using DSR.CrudCats.Crypto;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<CreateResponse.User> CreateAsync(CreateRequest.User userData);
        Task<FindCurrentResponse.User> FindCurrentAsync();
    }

    class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ICryptoHelper _cryptoHelper;
        private readonly IIdentityHelper _identityHelper;
        private readonly ILogger _logger;

        public UsersService(
            IUsersRepository usersRepository,
            ICryptoHelper cryptoHelper,
            IIdentityHelper identityHelper,
            ILogger<UsersService> logger)
        {
            logger.LogTrace("{method} > ...", nameof(UsersService));

            (_usersRepository, _cryptoHelper, _identityHelper, _logger) =
                (usersRepository, cryptoHelper, identityHelper, logger);

            logger.LogTrace("{method} <", nameof(UsersService));
        }

        public async Task<CreateResponse.User> CreateAsync(CreateRequest.User reqUser)
        {
            _logger.LogTrace("{method} > {@reqUser}", nameof(CreateAsync), reqUser);

            var exists = (await _usersRepository.Users.CountAsync(u => u.Email == reqUser.Email)) > 0;
            _logger.LogTrace("{method} ? exists {@exists}", nameof(CreateAsync), exists);

            if (exists)
            {
                _logger.LogWarning("{method} ! user already exists {@reqUser}", nameof(CreateAsync), reqUser);
                throw new ConflictException($"User {reqUser.Email} already exists.");
            }

            var (passwordHash, passwordSalt) = _cryptoHelper.HashPassword(reqUser.Password);

            var credentials = new Credentials(
                passwordHash: passwordHash,
                passwordSalt: passwordSalt
            );

            var user = new User(
                email: reqUser.Email,
                firstName: reqUser.FirstName,
                lastName: reqUser.LastName,
                credentials: credentials
            );

            _logger.LogTrace("{method} ? user {@user}", nameof(CreateAsync), user);

            await _usersRepository.Users
                .Add(user)
                .Context
                .SaveChangesAsync();

            _logger.LogTrace("{method} ? added user {@user}", nameof(CreateAsync), user);

            var resUser = new CreateResponse.User(
               email: user.Email,
               firstName: user.FirstName,
               lastName: user.LastName
           );

            _logger.LogTrace("{method} < {@resUser}", nameof(CreateAsync), resUser);
            return resUser;
        }

        public async Task<FindCurrentResponse.User> FindCurrentAsync()
        {
            _logger.LogTrace("{method} >", nameof(FindCurrentAsync));

            var userId = _identityHelper.CurrentUserId;
            _logger.LogTrace("{method} ? userId {userId}", nameof(FindCurrentAsync), userId);

            var user = await _usersRepository.Users.FindAsync(_identityHelper.CurrentUserId);
            _logger.LogTrace("{method} ? user {@user}", nameof(FindCurrentAsync), user);

            if (user == null) {
                _logger.LogError("{method} No current user {currentUserId}", nameof(FindCurrentAsync), userId);
                throw new InternalServerErrorException($"User not found for specified id {userId}!");
            }

            var resUser = new FindCurrentResponse.User(
                email: user.Email,
                firstName: user.FirstName,
                lastName: user.LastName
            );

            _logger.LogTrace("{method} < {@resUser}", resUser);
            return resUser;
        }
    }
}
