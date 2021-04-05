using JewelleryStore.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace JewelleryStore.Api.Authorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.User.Manage, policy => policy.Requirements.Add(new UserManagementPermissionRequirement()));
            });

            services.AddScoped<IAuthorizationHandler, UserManagementPermissionRequirementHandler>();

            return services;
        }
    }
}
