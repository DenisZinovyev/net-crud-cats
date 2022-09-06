namespace DSR.CrudCats.Auth
{
    using DSR.CrudCats.Crypto;
    using DSR.CrudCats.Users;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Authentication;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        Task<CreateTokenResponse.Token> CreateTokenAsync(CreateTokenRequest.Credentials reqCredentials);
    }

    class AuthService : IAuthService
    {
        private readonly AuthConfiguration _authConfiguration;
        private readonly IUsersRepository _usersRepository;
        private readonly ICryptoHelper _cryptoHelper;
        private readonly ILogger _logger;

        public AuthService(
            IUsersRepository repository,
            IOptions<AuthConfiguration> authConfiguration,
            ICryptoHelper cryptoHelper,
            ILogger<AuthService> logger)
        {
            logger.LogTrace("{method} > ...", nameof(AuthService));

            (_usersRepository, _authConfiguration, _cryptoHelper, _logger) =
                (repository, authConfiguration.Value, cryptoHelper, logger);

            logger.LogTrace("{method} <", nameof(AuthService));
        }

        public async Task<CreateTokenResponse.Token> CreateTokenAsync(CreateTokenRequest.Credentials reqCredentials)
        {
            _logger.LogTrace("{method} > {@reqCredentials}", nameof(CreateTokenAsync), reqCredentials);

            var user = await _usersRepository.Users
                .Include(u => u.Credentials)
                .FirstOrDefaultAsync(u => u.Email == reqCredentials.Email);

            _logger.LogTrace("{method} ? user {@user}", nameof(CreateTokenAsync), user);

            if (user == null)
            {
                _logger.LogWarning("{method} ! Unknown user {@reqCredentials}", nameof(CreateTokenAsync), reqCredentials);
                throw new AuthenticationException("Ivalid credentials!");
            }

            var credentials = user.Credentials;
            var reqPasswordHash = _cryptoHelper.HashPassword(reqCredentials.Password, credentials.PasswordSalt);

            if (!credentials.PasswordHash.SequenceEqual(reqPasswordHash))
            {
                _logger.LogWarning("{method} ! Passord doesn't match {@reqCredentials}", nameof(CreateTokenAsync), reqCredentials);
                throw new AuthenticationException("Ivalid credentials!");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfiguration.JwtIssuerSigningKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expiresIn = DateTime.UtcNow.AddMinutes(_authConfiguration.JwtLifetime);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _authConfiguration.JwtIssuer,
                audience: _authConfiguration.JwtAudience,
                claims: claims,
                expires: expiresIn,
                signingCredentials: signinCredentials
            );

            _logger.LogTrace("{method} ? token: {@token}", nameof(CreateTokenAsync), token);

            var resToken = new CreateTokenResponse.Token(new JwtSecurityTokenHandler().WriteToken(token));

            _logger.LogTrace("{method} < {@resToken}", nameof(CreateTokenAsync), resToken);
            return resToken;
        }
    }
}
