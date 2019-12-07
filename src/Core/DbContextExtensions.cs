using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HordeFlow.Core;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Core.Extensions
{
    public static class DbContextExtensions
    {
        public static ModelBuilder ApplyDesignTimeConfigurations(this ModelBuilder builder, string dir, bool useRowVersion)
        {
            var di = new DirectoryInfo(dir);
            var files = di.GetFiles("*.dll");

            foreach (FileInfo fi in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(fi.FullName);
                    if (!(assembly.FullName == "HordeFlow.Core" ||
                        assembly.FullName == "HordeFlow.Data" ||
                        assembly.FullName == "HordeFlow.Infrastructure"))
                    {
                        builder.ApplyConfigurations(useRowVersion, fi.FullName);
                    }
                }
                catch (Exception)
                {

                }
            }

            return builder;
        }
        public static ModelBuilder ApplyConfigurations(this ModelBuilder builder, bool useRowVersion, string path)
        {
            List<Type> configurations = GetEntityConfigurationsFromAssembly(path);

            foreach (Type m in configurations)
            {
                var c = Activator.CreateInstance(m, false);
                MethodInfo methodInfo = c.GetType().GetMethod("ApplyConfigurations");
                dynamic result = null;
                if (methodInfo != null)
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    object[] parametersArray = new object[] { builder, useRowVersion };
                    object classInstance = Activator.CreateInstance(c.GetType(), null);
                    result = methodInfo.Invoke(classInstance, parameters.Length == 0 ? null : parametersArray);
                }
            }

            return builder;
        }

        public static List<Type> GetEntityConfigurationsFromAssembly(string assemblyFile)
        {
            var assembly = Assembly.LoadFrom(assemblyFile);
            var configurations = assembly.GetExportedTypes()
                .Where(e => e.GetInterfaces()
                    .Any(c => c.GetTypeInfo().IsAssignableFrom(typeof(IEntityTypeConfigurationModule))))
                    .ToList();
            return configurations;
        }

        public static List<Type> GetTypesFromAssembly(string assemblyFile, Type type)
        {
            var assembly = Assembly.LoadFrom(assemblyFile);
            var types = assembly.GetExportedTypes()
                .Where(e => e.GetInterfaces()
                    .Any(c => c.GetTypeInfo().IsAssignableFrom(type)))
                    .ToList();
            return types;
        }
    }
}