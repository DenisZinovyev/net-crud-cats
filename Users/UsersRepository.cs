namespace DSR.CrudCats.Users
{
    using DSR.CrudCats.Persistence;
    using Microsoft.EntityFrameworkCore;

    public interface IUsersRepository
    {
        DbSet<User> Users { get; }
        DbContext Context { get; }
    }

    class UsersRepository : IUsersRepository
    {
        public DbSet<User> Users { get; }
        public DbContext Context { get; }

        public UsersRepository(IPersistenceContext context) =>
            (Users, Context) = (context.Context.Set<User>(), context.Context);
    }
}
