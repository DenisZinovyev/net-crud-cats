using DSR.Cats.Server.ApiDto.Requests;
using DSR.Cats.Server.ApiDto.Responses;
using DSR.Cats.Server.Services.Abstract;
using DSR.Cats.Server.Services.Exceptions;
using DSR.Cats.Server.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSR.Cats.Server.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cats")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly ICatsService _catsService;

        public CatController(ICatsService catsService)
        {
            _catsService = catsService;
        }

        #region api/cats

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var cats = await _catsService.GetAllAsync();
            var response = cats.Select(c => c.ToApiResponse());
            return Ok(response);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateCat([FromBody]CatRequest cat)
        {
            if (cat == null)
            {
                return BadRequest("Cat object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object: Validation error in a format of class-validator");
            }

            int id = await _catsService.CreateAsync(cat.Name, cat.BreedId);
            var catEntity = await _catsService.GetAsync(id);
            CatResponse response = catEntity.ToApiResponse();
            return StatusCode(StatusCodes.Status201Created, response);
        }

        #endregion

        #region api/cats/{id}

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<string>> Get(int id)
        {
            ApiDto.Responses.CatResponse response;
            try
            {
                var cat = await _catsService.GetAsync(id);
                response = cat.ToApiResponse();
            }
            catch (EntityNotFoundException)
            {
                return NotFound("Cat with provided id is not found");
            }

            return Ok(response);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> Put(int id, [FromBody]CatRequest cat)
        {
            if (cat == null)
            {
                return BadRequest("Cat object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object: Validation error in a format of class-validator");
            }

            try
            {
                await _catsService.UpdateAsync(id, cat.Name, cat.BreedId);
            }
            catch (EntityNotFoundException)
            {
                return NotFound("A cat with provided id is not found");
            }
            catch (AccessDeniedException)
            {
                return Forbid("An attempt to update a foreign cat");
            }
            
            return Ok(cat);
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _catsService.DeleteAsync(id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound("A cat with provided id is not found");
            }
            catch (AccessDeniedException)
            {
                return Forbid("An attempt to remove a foreign cat");
            }

            return NoContent();
        }

        #endregion
    }
}
