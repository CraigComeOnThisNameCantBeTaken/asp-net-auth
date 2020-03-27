using asp_net_auth.Authorization.Permissions.PermissionRequirementResolver;
using asp_net_auth.Authorization.Requirements.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

// todo: review for improvements
namespace asp_net_auth.Authorization
{
    public static class ServiceCollectionAddPermissionAuthorizationAuthorizationExtensions
    {
        public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services, Assembly assembly = default)
        {
            AddPermissionRequirements(services, assembly);
            AddAuthorizationHandlers(services, assembly);
            services.AddSingleton<IPermissionRequirementResolver, PermissionRequirementResolver>();
            return services;
        }
        private static IServiceCollection AddPermissionRequirements(IServiceCollection services, Assembly assembly = default)
        {
            if (assembly == default)
                assembly = Assembly.GetEntryAssembly();

            var serviceType = typeof(PermissionRequirement);
            var requirements = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => serviceType.IsAssignableFrom(t))
                .ToArray();

            foreach (var requirement in requirements)
            {
                services.AddSingleton(serviceType, requirement);
            }

            return services;
        }

        private static IServiceCollection AddAuthorizationHandlers(IServiceCollection services, Assembly assembly = default)
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
                // todo: this can be improved.
                if (handler.IsGenericType)
                {
                    var requirementType = handler.GetGenericArguments()[0];
                    if (requirementType.IsAssignableFrom(typeof(IAuthorizationRequirement)))
                    {
                        services.AddSingleton(typeof(IAuthorizationRequirement), requirementType);
                    }
                }
                //

                services.AddSingleton(serviceType, handler);
            }

            return services;
        }
    }
}
