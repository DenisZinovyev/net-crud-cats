namespace DSR.CrudCats.Auth
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController, Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("tokens")]
        public async Task<ActionResult<CreateTokenResponse.Token>> CreateToken([FromBody]CreateTokenRequest.Credentials req)
        {
            var res = await _authService.CreateTokenAsync(req);
            return Ok(res);
        }
    }
}