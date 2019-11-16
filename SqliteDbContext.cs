using i21Apis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace i21Apis
{
    public class SqliteDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly Tenant tenant;

        public SqliteDbContext(DbContextOptions<SqliteDbContext> options)
            : base(options)
        {

        }

        public SqliteDbContext(IConfiguration configuration, Tenant tenant, DbContextOptions<SqliteDbContext> options)
            : this(options)
        {
            this.configuration = configuration;
            this.tenant = tenant;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = tenant.ConnectionString;
            optionsBuilder.UseSqlite(connectionString, options => { });
        }

        public DbSet<tblARCustomer> tblARCustomer { get; set; }
        public DbSet<tblSMCompanyLocation> tblSMCompanyLocation { get; set; }
    }
}