namespace DSR.CrudCats.Breeds
{
    using DSR.CrudCats.Persistence;
    using System.ComponentModel.DataAnnotations;

    [Persistent]
    public class Breed
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        private Breed() { }
        public Breed(string name) => Name = name;
    }
}
