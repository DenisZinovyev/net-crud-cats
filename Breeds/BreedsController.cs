namespace DSR.CrudCats.Breeds
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController, Route("api/v1/[controller]"), Authorize]
    public class BreedsController : ControllerBase
    {
        private readonly IBreedsService _breedsService;

        public BreedsController(IBreedsService breedService) =>
            _breedsService = breedService;

        [HttpGet]
        public async Task<ActionResult<FindAllResponse.Breed[]>> FindAll()
        {
            var res = await _breedsService.FindAllAsync();
            return Ok(res);
        }
    }
}
