namespace DSR.CrudCats.Cats.CreateRequest
{
    using System.ComponentModel.DataAnnotations;

    public class Cat
    {
        [Required]
        [NameCheck]
        public string Name { get; set; }

        [Required]
        public int BreedId { get; set; }
    }
}
