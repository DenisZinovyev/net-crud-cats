namespace DSR.CrudCats.Users.CreateRequest
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
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
