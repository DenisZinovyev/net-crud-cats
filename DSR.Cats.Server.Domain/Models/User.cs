using System.Collections.Generic;

namespace DSR.Cats.Server.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICollection<Cat> Cats { get; set; }
    }
}
