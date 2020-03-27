using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace asp_net_auth.Authorization
{
    public static class ServiceCollectionAuthorizationHandlerExtensions
    {
        public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services, Assembly assembly = default)
        {
            var serviceType = typeof(IAuthorizationHandler);

            if (assembly == default)
                assembly = Assembly.GetEntryAssembly();

            var handlers = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => !t.IsInterface)
                .Where(t => serviceType.IsAssignableFrom(t))
                .ToArray();

            foreach (var handler in handlers)
            {
                services.AddSingleton(serviceType, handler);
            }

            return services;
        }
    }
}
