using DSR.Cats.Server.ApiDto.Requests;
using DSR.Cats.Server.ApiDto.Responses;
using DSR.Cats.Server.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace DSR.Cats.Server.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth/token")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            JwtSecurityToken token;
            try
            {
                token = await _authService.LoginAsync(user.Email, user.Password);
            }
            catch (AuthenticationException)
            {
                return NotFound("A user with provided email and password is not found");
            }

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new TokenResponse
            {
                AuthToken = tokenString,
                ExpiresIn = (long)token.Payload["exp"],
            });
        }
    }
}