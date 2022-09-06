namespace DSR.CrudCats.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;

    public interface IPersistenceContext: IDisposable
    {
        DbContext Context { get; }
    }

    public class PersistenceContext : DbContext, IPersistenceContext
    {
        private readonly ILogger<PersistenceContext> _logger;
        public DbContext Context => this;

        public PersistenceContext(DbContextOptions<PersistenceContext> options, ILogger<PersistenceContext> logger) : base(options)
        {
            logger.LogTrace("{method} > ... ", nameof(PersistenceContext));
            _logger = logger;
            logger.LogTrace("{method} <", nameof(PersistenceContext));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogTrace("{method} > ...", nameof(OnModelCreating));

            var entityMethod = typeof(ModelBuilder).GetMethod("Entity", System.Type.EmptyTypes);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var entityTypes = assembly
                  .GetTypes()
                  .Where(t =>
                    t.GetCustomAttributes(typeof(PersistentAttribute), inherit: true).Any());

                foreach (var type in entityTypes)
                {
                    _logger.LogInformation("Registering entity {type}", type);
                    entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
                }
            }

            base.OnModelCreating(modelBuilder);
            _logger.LogTrace("{method} < ", nameof(OnModelCreating));
        }

        public override void Dispose()
        {
             _logger.LogTrace("{method} > ", nameof(Dispose));
            base.Dispose();
             _logger.LogTrace("{method} < ", nameof(Dispose));
        }
    }

    public interface IPersistenceContextFactory
    {
        IPersistenceContext CreateContext();
    }

    public class PersistenceContextFactory : IPersistenceContextFactory
    {
        private readonly DbContextOptions<PersistenceContext> _options;
        private readonly ILogger<PersistenceContextFactory> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public PersistenceContextFactory(
            DbContextOptions<PersistenceContext> options,
            ILogger<PersistenceContextFactory> logger,
            ILoggerFactory loggerFactory)
        {
            logger.LogTrace("{method} > ...", nameof(PersistenceContextFactory));
            (_options, _logger, _loggerFactory) = (options, logger, loggerFactory);
            logger.LogTrace("{method} <", nameof(PersistenceContextFactory));
        }

        public IPersistenceContext CreateContext()
        {
            _logger.LogTrace("{method} >", nameof(PersistenceContextFactory));

            var context = new PersistenceContext(_options, _loggerFactory.CreateLogger<PersistenceContext>());

            _logger.LogTrace("{method} < {context}", nameof(PersistenceContextFactory), context);
            return context;
        }
    }
}
