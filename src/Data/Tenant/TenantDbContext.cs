using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HordeFlow.Core;
using HordeFlow.Core.Extensions;
using System;

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var provider = configuration.GetValue<DatabaseProvider>("DatabaseProvider");
            var edition = configuration.GetValue("SQLEdition", "Latest");

            builder.ApplyDesignTimeConfigurations(AppDomain.CurrentDomain.BaseDirectory, provider == DatabaseProvider.SqlServer);

            builder.Entity<Tenant>().Property(e => e.Name).HasMaxLength(50);
            builder.Entity<Tenant>().Property(e => e.DatabaseProvider).HasConversion<string>();

            base.OnModelCreating(builder);
        }

        public DbSet<Tenant> Tenant { get; set; }
    }
}