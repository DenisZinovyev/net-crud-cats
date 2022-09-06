using DSR.Cats.Server.ApiDto.Requests;
using DSR.Cats.Server.Services.Abstract;
using DSR.Cats.Server.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSR.Breeds.Server.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/breeds")]
    [ApiController]
    public class BreedController : ControllerBase
    {
        private readonly IBreedsService _breedService;

        public BreedController(IBreedsService breedService)
        {
            _breedService = breedService;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var breeds = await _breedService.GetAllAsync();
            var response = breeds.Select(b => b.ToApiResponse());
            return Ok(response);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateBreed([FromBody]BreedRequest breed)
        {
            if (breed == null)
            {
                return BadRequest("Breed object is null");
            }

            var foundBreed = await _breedService.FindByNameAsync(breed.Name);
            if (foundBreed != null)
            {
                ModelState.AddModelError(nameof(breed.Name), "Breed with same name already exist");
            }

            if (!ModelState.IsValid)
            {
                var validationProblemDetails = new ValidationProblemDetails(ModelState);
                return BadRequest(validationProblemDetails);
            }

            await _breedService.CreateAsync(breed.Name);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
