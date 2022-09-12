using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System;
using System.Linq;

namespace AstroOdyssey
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddFactories(this IServiceCollection serviceCollection)
        {
            var allFactories = Assembly.GetAssembly(typeof(CelestialObjectFactory))?.GetTypes().Where(type => !type.IsInterface && type.Name.EndsWith("Factory"));

            if (allFactories is not null)
            {
                foreach (var item in allFactories)
                {
                    Type serviceType = item.GetTypeInfo().ImplementedInterfaces.First();
                    serviceCollection.AddSingleton(serviceType, item);
                }
            }

            return serviceCollection;
        }

        public static IServiceCollection AddHelpers(this IServiceCollection serviceCollection)
        {
            var allFactories = Assembly.GetAssembly(typeof(AudioHelper))?.GetTypes().Where(type => !type.IsInterface && type.Name.EndsWith("Helper"));

            if (allFactories is not null)
            {
                foreach (var item in allFactories)
                {
                    Type serviceType = item.GetTypeInfo().ImplementedInterfaces.First();
                    serviceCollection.AddSingleton(serviceType, item);
                }
            }

            return serviceCollection;
        }
    }
}
