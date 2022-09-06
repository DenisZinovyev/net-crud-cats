namespace DSR.CrudCats.Breeds
{
    using DSR.CrudCats.Persistence;
    using Microsoft.EntityFrameworkCore;

    public interface IBreedsRepository
    {
        DbSet<Breed> Breeds { get; }
        DbContext Context { get; }
    }

    class BreedsRepository : IBreedsRepository
    {
        public DbSet<Breed> Breeds { get; }
        public DbContext Context { get; }
       
        public BreedsRepository(IPersistenceContext context) =>
            (Breeds, Context) = (context.Context.Set<Breed>(), context.Context);
    }
}
