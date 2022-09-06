namespace DSR.CrudCats.Cats.FindAllResponse
{
    public class Cat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BreedId { get; set; }

        public Cat(int id, string name, int breedId) =>
            (Id, Name, BreedId) = (id, name, breedId);
    }
}
