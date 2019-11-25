using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HordeFlow.Core;

namespace HordeFlow.Data.Catalog
{
    public class SqlServerCatalogDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public SqlServerCatalogDbContext(IConfiguration configuration, DbContextOptions<SqlServerCatalogDbContext> options)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("Catalog");
            var edition = configuration.GetValue("SQLEdition", "Latest");

            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.UseRowNumberForPaging(edition.ToUpper().Equals("SQL2008R2"));
            });
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Tenant>().Property(e => e.Name).HasMaxLength(50);
            builder.Entity<Tenant>().Property(e => e.DatabaseProvider).HasConversion<string>();
        }

        public DbSet<Tenant> Tenant { get; set; }
    }
}