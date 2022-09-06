using DSR.Cats.Server.ApiDto.Requests;
using DSR.Cats.Server.Services.Abstract;
using DSR.Cats.Server.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DSR.Cats.Server.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Get()
        {
            var currentUser = await _userService.FindByEmailAsync(User.Identity.Name);
            var response = currentUser.ToApiResponse();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]RegisterRequest userCreate)
        {
            if (userCreate == null)
            {
                return BadRequest("User data is null");
            }

            var foundUser = await _userService.FindByEmailAsync(userCreate.Email);
            if (foundUser != null)
            {
                ModelState.AddModelError(nameof(userCreate.Email), "Email already exist");
            }

            if (!ModelState.IsValid)
            {
                var validationProblemDetails = new ValidationProblemDetails(ModelState);
                return BadRequest(validationProblemDetails);
            }

            await _userService.CreateAsync(userCreate.Email, userCreate.FirstName, userCreate.LastName, userCreate.Password);
            return NoContent();            
        }
    }
}