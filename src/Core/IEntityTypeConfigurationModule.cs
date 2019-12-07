using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Core
{
    public interface IEntityTypeConfigurationModule : IModule
    {
        void ApplyConfigurations(ModelBuilder builder, bool useRowVersion);
    }
}