namespace DSR.CrudCats.Breeds
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IBreedsService
    {
        Task<FindAllResponse.Breed[]> FindAllAsync();
    }

    class BreedsService : IBreedsService
    {
        private readonly IBreedsRepository _breedsRepository;
        private readonly ILogger _logger;

        public BreedsService(IBreedsRepository breedsRepository, ILogger<BreedsService> logger)
        {
            logger.LogTrace("{method} > ...", nameof(BreedsService));
            (_breedsRepository, _logger) = (breedsRepository, logger);
            logger.LogTrace("{method} <", nameof(BreedsService));
        }

        public void Initialize(BreedsConfiguration cfg)
        {
            _logger.LogTrace("{method} > {@cfg}", nameof(Initialize), cfg);

            var breedsExists = _breedsRepository.Breeds.Count() > 0;
            _logger.LogTrace("{method} ? breedsExists {breedsExists}", nameof(Initialize), breedsExists);

            if (breedsExists)
            {
                _logger.LogInformation("{method} Breeds already published", nameof(Initialize));
                _logger.LogTrace("{method} <", nameof(Initialize));
                return;
            }

            var breeds = cfg
                .Breeds
                .Select(b =>
                    new Breed(
                        name: b.Name
                    ))
                .ToArray();

            _logger.LogTrace("{method} ? breeds {@breeds}", nameof(Initialize), breeds);

            _breedsRepository.Breeds.AddRange(breeds);
            _logger.LogTrace("{method} ? added breeds {@breeds}", nameof(Initialize), breeds);

            _breedsRepository.Context.SaveChanges();
            _logger.LogInformation("{method} Breeds were published", nameof(Initialize));

            _logger.LogTrace("{method} <", nameof(Initialize));
        }

        public async Task<FindAllResponse.Breed[]> FindAllAsync()
        {
            _logger.LogTrace("{method} >", nameof(FindAllAsync));

            var breeds = await _breedsRepository.Breeds.ToArrayAsync();
            _logger.LogTrace("{method} ? breeds {@breeds}", nameof(FindAllAsync), breeds);

            var resBreeds = breeds
                .Select(b =>
                    new FindAllResponse.Breed(
                        id: b.Id,
                        name: b.Name
                    ))
                .ToArray();

            _logger.LogTrace("{method} < {@resBreeds}", nameof(FindAllAsync), resBreeds);
            return resBreeds;
        }
    }
}
