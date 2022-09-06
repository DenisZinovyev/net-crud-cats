namespace DSR.CrudCats.Cats
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController, Route("api/v1/[controller]"), Authorize]
    public class CatsController : ControllerBase
    {
        private readonly ICatsService _catsService;

        public CatsController(ICatsService catsService) => _catsService = catsService;

        [HttpGet]
        public async Task<ActionResult<FindResponse.Cat>> FindAll()
        {
            var resCats = await _catsService.FindAllAsync();
            return Ok(resCats);
        }

        [HttpPost]
        public async Task<ActionResult<CreateResponse.Cat>> Create([FromBody]CreateRequest.Cat reqCat)
        {
            var resCat = await _catsService.CreateAsync(reqCat);
            return CreatedAtAction(nameof(Find), new { id = resCat.Id }, resCat);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FindResponse.Cat>> Find(int id)
        {
            var resCat = await _catsService.FindAsync(id);
            return Ok(resCat);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateResponse.Cat>> Update(int id, [FromBody]UpdateRequest.Cat reqCat)
        {
            var resCat = await _catsService.UpdateAsync(id, reqCat);
            return Ok(resCat);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _catsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
