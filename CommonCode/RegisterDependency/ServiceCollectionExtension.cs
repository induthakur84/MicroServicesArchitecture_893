using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RegisterDependency
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services,
            string assemblyName)
        {

            var types = Assembly.Load(assemblyName).GetTypes(); 


            var interfaces = types.Where(t=>t.GetCustomAttribute
            <RegisterDependencyAttribute>() != null  && t.IsInterface);


            foreach (var @interface in interfaces)
            {
                var @class= GetImplementationClass(types, @interface);
            }

        }

        public static Type GetImplementationClass(Type[] types, Type interfaceType)
        {
            var @class= types.FirstOrDefault(t=>t.GetTypeInfo().ImplementedInterfaces.Any(i=>i == interfaceType));

            return @class  ?? throw new NotImplementedException($"No class Implements this interface for {interfaceType.Name}");
        }
    }
}
