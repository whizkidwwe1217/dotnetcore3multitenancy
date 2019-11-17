using Microsoft.EntityFrameworkCore;

namespace i21Apis.Data
{
    public interface IDbContextConfigurationBuilder
    {
        void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
    }
}