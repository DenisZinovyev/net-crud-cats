using DSR.Cats.Server.Services.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using DSR.Cats.Server.Services.Models;
using System.Security.Authentication;
using DSR.Cats.Server.Domain.Models;
using System.Threading.Tasks;

namespace DSR.Cats.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> LoginAsync(string email, string password)
        {
            User foundUser = await _userService.FindByEmailAsync(email);
            if (foundUser == null)
            {
                throw new AuthenticationException($"Email {email} not found");
            }
            if (foundUser.Password != password)
            {
                throw new AuthenticationException("Invalid password");
            }

            var authConfiguration = GetAuthConfiguration();
            var secretKey = new SymmetricSecurityKey(authConfiguration.IssuerSigningKey);
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expiresIn = DateTime.Now.AddMinutes(authConfiguration.TokenLifetimeMinutes);
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, email));
            var token = new JwtSecurityToken(
                issuer: authConfiguration.Issuer,
                audience: authConfiguration.Audience,
                claims: claims,
                expires: expiresIn,
                signingCredentials: signinCredentials
            );

            return token;
        }

        public AuthConfiguration GetAuthConfiguration()
        {
            var issuerSigningKeyStr = _configuration.GetValue<string>("Authentication:IssuerSigningKey");
            var issuerSigningKey = Encoding.UTF8.GetBytes(issuerSigningKeyStr);
            var authConfiguration = new AuthConfiguration
            {
                TokenLifetimeMinutes = _configuration.GetValue<int>("Authentication:TokenLifetimeMinutes"),
                IssuerSigningKey = issuerSigningKey,
                Issuer = _configuration.GetValue<string>("Authentication:Issuer"),
                Audience = _configuration.GetValue<string>("Authentication:Audience"),
            };

            return authConfiguration;
        }
    }
}
