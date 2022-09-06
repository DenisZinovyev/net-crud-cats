using System.Collections.Generic;

namespace DSR.Cats.Server.Domain.Models
{
    public class Breed
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Cat> Cats { get; set; }
    }
}
