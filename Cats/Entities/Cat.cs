namespace DSR.CrudCats.Cats
{
    using DSR.CrudCats.Breeds;
    using DSR.CrudCats.Persistence;
    using DSR.CrudCats.Users;
    using System.ComponentModel.DataAnnotations;

    [Persistent]
    public class Cat
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int BreedId { get; set; }
        public Breed Breed { get; set; }

        [Required]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        private Cat() { }

        public Cat(string name, int breedId, int ownerId) =>
            (Name, BreedId, OwnerId) = (name, breedId, ownerId);
    }
}
