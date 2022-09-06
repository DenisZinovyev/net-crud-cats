namespace DSR.CrudCats.Breeds.FindAllResponse
{
    public class Breed
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Breed(int id, string name) =>
            (Id, Name) = (id, name);
    }
}
