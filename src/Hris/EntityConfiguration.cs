using HordeFlow.Core;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Hris
{
    public class EntityConfiguration : IEntityTypeConfigurationModule
    {
        public string Name => throw new System.NotImplementedException();

        public void ApplyConfigurations(ModelBuilder builder, bool useRowVersion)
        {
        }
    }
}