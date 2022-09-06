namespace DSR.CrudCats.Cats
{
    using DSR.CrudCats.Auth;
    using DSR.CrudCats.Breeds;
    using DSR.CrudCats.Common;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICatsService
    {
        Task<FindAllResponse.Cat[]> FindAllAsync();
        Task<CreateResponse.Cat> CreateAsync(CreateRequest.Cat reqCat);
        Task<FindResponse.Cat> FindAsync(int id);
        Task<UpdateResponse.Cat> UpdateAsync(int id, UpdateRequest.Cat reqCat);
        Task DeleteAsync(int id);
    }

    class CatsService : ICatsService
    {
        private readonly IBreedsRepository _breedsRepository;
        private readonly ICatsRepository _catsRepository;
        private readonly IIdentityHelper _identityHelper;
        private readonly ILogger _logger;

        public CatsService(
            IBreedsRepository breedsRepository,
            ICatsRepository catsRepository,
            IIdentityHelper identityHelper,
            ILogger<CatsService> logger)
        {
            logger.LogTrace("{method} > ...", nameof(CatsService));
            (_breedsRepository, _catsRepository, _identityHelper, _logger) = (breedsRepository, catsRepository, identityHelper, logger);
            logger.LogTrace("{method} <", nameof(CatsService));
        }

        public async Task<FindResponse.Cat> FindAsync(int id)
        {
            _logger.LogTrace("{method} > {Id}", nameof(FindAsync), id);

            Cat cat = await LoadAndVerifyOwnedCatExistence(id);
            _logger.LogTrace("{method} ? cat {@cat}", nameof(FindAsync), cat);

            var resCat = new FindResponse.Cat(
                id: cat.Id,
                name: cat.Name,
                breedId: cat.BreedId
            );

            _logger.LogTrace("{method} < {@resCat}", nameof(FindAsync), resCat);
            return resCat;
        }

        public async Task<FindAllResponse.Cat[]> FindAllAsync()
        {
            _logger.LogTrace("{method} >", nameof(FindAllAsync));

            var cats = await _catsRepository.Cats
                .Where(c => c.OwnerId == _identityHelper.CurrentUserId)
                .ToArrayAsync();

            _logger.LogTrace("{method} ? cats {@cats}", nameof(FindAllAsync), cats);

            var resCats = cats
                .Select(c =>
                    new FindAllResponse.Cat(
                        id: c.Id,
                        name: c.Name,
                        breedId: c.BreedId
                    ))
                .ToArray();

            _logger.LogTrace("{method} < {@resCats}", nameof(FindAllAsync), resCats);
            return resCats;
        }

        public async Task<CreateResponse.Cat> CreateAsync(CreateRequest.Cat reqCat)
        {
            _logger.LogTrace("{method} > {@reqCat}", nameof(CreateAsync), reqCat);

            await VerifyBreedExistence(reqCat.BreedId);

            Cat cat = new Cat(
                name: reqCat.Name,
                breedId: reqCat.BreedId,
                ownerId: _identityHelper.CurrentUserId
            );

            _logger.LogTrace("{method} ? cat {@cat}", nameof(CreateAsync), cat);

            await _catsRepository.Cats
                .Add(cat)
                .Context
                .SaveChangesAsync();

            _logger.LogTrace("{method} ? added cat {@cat}", nameof(CreateAsync), cat);

            var resCat = new CreateResponse.Cat(
                id: cat.Id,
                name: cat.Name,
                breedId: cat.BreedId
            );

            _logger.LogTrace("{method} < {@resCat}", nameof(CreateAsync), resCat);
            return resCat;
        }

        public async Task<UpdateResponse.Cat> UpdateAsync(int id, UpdateRequest.Cat reqCat)
        {
            _logger.LogTrace("{method} > {id} {@reqCat}", nameof(UpdateAsync), id, reqCat);

            await VerifyBreedExistence(reqCat.BreedId);

            var cat = await LoadAndVerifyOwnedCatExistence(id);
            _logger.LogTrace("{method} ? cat {@cat}", nameof(UpdateAsync), cat);

            (cat.Name, cat.BreedId) = (reqCat.Name, reqCat.BreedId);

            await _catsRepository.Cats
                .Update(cat)
                .Context
                .SaveChangesAsync();

            _logger.LogTrace("{method} ? updated cat {@cat}", nameof(UpdateAsync), cat);

            var resCat = new UpdateResponse.Cat(
                id: cat.Id,
                name: cat.Name,
                breedId: cat.BreedId
            );

            _logger.LogTrace("{method} < {@resCat}", nameof(UpdateAsync), resCat);
            return resCat;
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogTrace("{method} > {id}", nameof(DeleteAsync), id);

            var cat = await LoadAndVerifyOwnedCatExistence(id);
            _logger.LogTrace("{method} ? {@cat}", nameof(DeleteAsync), cat);

            await _catsRepository.Cats
                .Remove(cat)
                .Context
                .SaveChangesAsync();

            _logger.LogTrace("{method} <", nameof(DeleteAsync));
        }

        private async Task<Cat> LoadAndVerifyOwnedCatExistence(int id)
        {
            _logger.LogTrace("{method} > {id}", nameof(LoadAndVerifyOwnedCatExistence), id);

            Cat cat = await _catsRepository.Cats
                .Where(c => c.Id == id && c.OwnerId == _identityHelper.CurrentUserId)
                .FirstAsync();

            _logger.LogTrace("{method} ? cat {@cat}", nameof(LoadAndVerifyOwnedCatExistence), cat);

            if (cat == null)
            {
                _logger.LogWarning("{method} ! cat not found {id}", nameof(LoadAndVerifyOwnedCatExistence), id);
                throw new NotFoundException($"Cat with id {id} is not found!");
            }

            _logger.LogTrace("{method} < {@cat}", nameof(LoadAndVerifyOwnedCatExistence), cat);
            return cat;
        }

        private async Task VerifyBreedExistence(int id)
        {
            _logger.LogTrace("{method} > {id}", nameof(VerifyBreedExistence), id);

            var breedExists =
                (await _breedsRepository.Breeds
                    .Where(b => b.Id == id)
                    .CountAsync()) > 0;

            _logger.LogTrace("{method} ? breedExists {breedExists}", nameof(VerifyBreedExistence), breedExists);

            if (!breedExists)
            {
                _logger.LogWarning("{method} ! unknown breed {id}", nameof(VerifyBreedExistence), id);
                throw new BadRequestException($"Unknown breed {id}!");
            }
        }
    }
}
