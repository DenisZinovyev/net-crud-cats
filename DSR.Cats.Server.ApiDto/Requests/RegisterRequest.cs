using DSR.Cats.Server.ApiDto.Requests.Validators;
using System.ComponentModel.DataAnnotations;

namespace DSR.Cats.Server.ApiDto.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum password length is 6 symbols")]
        public string Password { get; set; }

        [Required]
        [NameCheck]
        public string FirstName { get; set; }

        [Required]
        [NameCheck]
        public string LastName { get; set; }
    }
}
