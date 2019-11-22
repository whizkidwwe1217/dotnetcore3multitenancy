using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data
{
    public abstract class TenantDbContext<TDbContext> : DbContext
        where TDbContext : DbContext
    {
        protected readonly IConfiguration configuration;
        protected readonly Tenant tenant;
        protected readonly IDbContextConfigurationBuilder builder;

        public TenantDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {

        }

        public TenantDbContext(IConfiguration configuration, Tenant tenant,
            IDbContextConfigurationBuilder builder, DbContextOptions<TDbContext> options)
            : this(options)
        {
            this.configuration = configuration;
            this.tenant = tenant;
            this.builder = builder;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (builder != null) builder.Build(optionsBuilder);
        }

        public DbSet<tblARCustomer> tblARCustomer { get; set; }
        public DbSet<tblSMCompanyLocation> tblSMCompanyLocation { get; set; }
    }
}