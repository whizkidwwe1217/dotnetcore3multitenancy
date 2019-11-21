using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data
{
    public class CatalogDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public CatalogDbContext(IConfiguration configuration, DbContextOptions<CatalogDbContext> options)
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

        public DbSet<Tenant> Tenant { get; set; }
    }
}