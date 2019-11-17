using i21Apis.Data;
using i21Apis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace i21Apis.Data
{
    public class TenantDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly Tenant tenant;
        private readonly IDbContextConfigurationBuilder builder;

        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {

        }

        public TenantDbContext(IConfiguration configuration, Tenant tenant,
            IDbContextConfigurationBuilder builder, DbContextOptions<TenantDbContext> options)
            : this(options)
        {
            this.configuration = configuration;
            this.tenant = tenant;
            this.builder = builder;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (builder != null) builder.OnConfiguring(optionsBuilder);
        }

        public DbSet<tblARCustomer> tblARCustomer { get; set; }
        public DbSet<tblSMCompanyLocation> tblSMCompanyLocation { get; set; }
    }
}