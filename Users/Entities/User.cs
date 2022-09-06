namespace DSR.CrudCats.Users
{
    using System.ComponentModel.DataAnnotations;
    using DSR.CrudCats.Persistence;

    [Persistent]
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public Credentials Credentials { get; set; }

        private User() { }

        public User(string email, string firstName, string lastName, Credentials credentials) =>
          (Email, FirstName, LastName, Credentials) = (email, firstName, lastName, credentials);
    }
}
