using DSR.Cats.Server.ApiDto.Requests.Validators;
using System.ComponentModel.DataAnnotations;

namespace DSR.Cats.Server.ApiDto.Requests
{
    public class CatRequest
    {
        [Required]
        [NameCheck]
        public string Name { get; set; }

        [Required]
        public int BreedId { get; set; }
    }
}
