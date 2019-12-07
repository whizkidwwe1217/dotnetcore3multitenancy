using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HordeFlow.Infrastructure.Extensions
{
    public static class StartupExtensions
    {
        public static IMvcBuilder AddCoreAssemblies(this IMvcBuilder builder, IConfiguration configuration)
        {
            var assemblies = ModuleExtensions.GetReferencingAssemblies(typeof(HordeFlow.Core.IModule).Namespace); // TODO: Needs enhancements
            // var assembly = assemblies.First();
            // builder.AddApplicationPart(assembly);
            foreach (Assembly assembly in assemblies)
            {
                builder.AddApplicationPart(assembly);
            }
            builder.AddApplicationPart(typeof(BaseStartup).Assembly);
            return builder;
        }
    }
}