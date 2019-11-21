using System;
using HordeFlow.Data;
using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace HordeFlow.Data
{
    public class TenantMySqlDbContext : TenantDbContext<TenantMySqlDbContext>
    {
        public TenantMySqlDbContext(DbContextOptions<TenantMySqlDbContext> options)
            : base(options) { }

        public TenantMySqlDbContext(IConfiguration configuration, Tenant tenant,
            IDbContextConfigurationBuilder builder, DbContextOptions<TenantMySqlDbContext> options)
            : base(configuration, tenant, builder, options) { }
    }
}