namespace DSR.CrudCats.Auth.CreateTokenRequest
{
    using System.ComponentModel.DataAnnotations;

    public class Credentials
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

}