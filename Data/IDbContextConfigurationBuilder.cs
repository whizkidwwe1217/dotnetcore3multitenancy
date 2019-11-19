using Microsoft.EntityFrameworkCore;

namespace i21Apis.Data
{
    public interface IDbContextConfigurationBuilder
    {
        DbContextOptionsBuilder Build(DbContextOptionsBuilder optionsBuilder);
    }
}