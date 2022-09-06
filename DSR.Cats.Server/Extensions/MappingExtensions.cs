using DSR.Cats.Server.ApiDto.Responses;

namespace DSR.Cats.Server.WebApi.Extensions
{
    public static class MappingExtensions
    {
        public static CatResponse ToApiResponse(this Domain.Models.Cat entity)
        {
            return new CatResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                BreedId = entity.BreedId,
            };
        }

        public static BreedResponse ToApiResponse(this Domain.Models.Breed entity)
        {
            return new BreedResponse
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }

        public static UserInfoResponse ToApiResponse(this Domain.Models.User entity)
        {
            return new UserInfoResponse
            {
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
            };
        }
    }
}
