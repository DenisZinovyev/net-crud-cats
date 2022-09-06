namespace DSR.CrudCats.Breeds
{
    public class BreedsConfiguration
    {
        public Breed[] Breeds { get; set; }

        public class Breed
        {
            public string Name { get; set; }
        }
    }
}
