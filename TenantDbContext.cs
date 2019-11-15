using i21Apis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace i21Apis
{
    public class TenantDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly Tenant tenant;

        public TenantDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public TenantDbContext(IConfiguration configuration, Tenant tenant, DbContextOptions options)
            : this(options)
        {
            this.configuration = configuration;
            this.tenant = tenant;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = tenant.ConnectionString;
            var edition = configuration.GetValue("SQLEdition", "Latest");
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.UseRowNumberForPaging(edition.ToUpper().Equals("SQL2008R2"));
            });
        }

        public DbSet<tblARCustomer> tblARCustomer { get; set; }
        public DbSet<tblSMCompanyLocation> tblSMCompanyLocation { get; set; }
    }
}