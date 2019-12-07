using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HordeFlow.Core;

namespace HordeFlow.Data.Catalog
{
    public abstract class CatalogDbContext<TCatalogDbContext> : DbContext
        where TCatalogDbContext : DbContext
    {
        protected readonly IConfiguration configuration;

        public CatalogDbContext(IConfiguration configuration, DbContextOptions<TCatalogDbContext> options)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Configure(optionsBuilder);
        }

        protected abstract void Configure(DbContextOptionsBuilder optionsBuilder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Tenant>().Property(e => e.Name).HasMaxLength(50);
            builder.Entity<Tenant>().Property(e => e.DatabaseProvider).HasConversion<string>();
        }

        public DbSet<Tenant> Tenant { get; set; }
    }
}