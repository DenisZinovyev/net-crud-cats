using DSR.Cats.Server.Services.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace DSR.Cats.Server.Services.Abstract
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> LoginAsync(string email, string password);
        AuthConfiguration GetAuthConfiguration();
    }
}
