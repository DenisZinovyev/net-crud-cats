namespace DSR.Cats.Server.Domain.Models
{
    public class Cat
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BreedId { get; set; }

        public Breed Breed { get; set; }

        public int? OwnerId { get; set; }

        public User Owner { get; set; }
    }
}
