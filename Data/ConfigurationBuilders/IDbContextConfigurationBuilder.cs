using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Data
{
    public interface IDbContextConfigurationBuilder
    {
        DbContextOptionsBuilder Build(DbContextOptionsBuilder optionsBuilder);
    }
}