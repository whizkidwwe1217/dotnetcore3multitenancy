using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace HordeFlow.Infrastructure.Extensions
{
    public static class ModuleExtensions
    {
        public static List<Assembly> GetLoadedAssemblies(string assemblyName)
        {
            var assemblies = GetReferencingAssemblies(assemblyName).ToList();
            return assemblies;
        }

        public static IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateLibrary(library, assemblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }

            return assemblies;
        }

        private static bool IsCandidateLibrary(RuntimeLibrary library, string assemblyName)
        {
            return library.Dependencies.Any(d => d.Name.StartsWith(assemblyName));
        }
    }
}