using DSR.Cats.Server.ApiDto.Requests.Validators;
using System.ComponentModel.DataAnnotations;

namespace DSR.Cats.Server.ApiDto.Requests
{
    public class BreedRequest
    {
        [Required]
        [NameCheck]
        public string Name { get; set; }
    }
}