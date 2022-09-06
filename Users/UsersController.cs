namespace DSR.CrudCats.Users
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController, Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]CreateRequest.User reqUser)
        {
            var resUser = await _userService.CreateAsync(reqUser);
            return CreatedAtAction(nameof(FindCurrent), null, resUser);
        }

        [HttpGet("me"), Authorize]
        public async Task<ActionResult<FindCurrentResponse.User>> FindCurrent()
        {
            var resUser = await _userService.FindCurrentAsync();
            return Ok(resUser);
        }
    }
}