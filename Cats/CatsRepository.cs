namespace DSR.CrudCats.Cats
{
    using DSR.CrudCats.Persistence;
    using Microsoft.EntityFrameworkCore;

    public interface ICatsRepository
    {
        DbSet<Cat> Cats { get; }
        DbContext Context { get; }
    }

    class CatsRepository : ICatsRepository
    {
        public DbSet<Cat> Cats { get; }
        public DbContext Context { get; }

        public CatsRepository(IPersistenceContext context) =>
            (Cats, Context) = (context.Context.Set<Cat>(), context.Context);
    }
}
