using JewelleryStore.Application.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace JewelleryStore.Api.Authorization.Requirements
{
    public class UserManagementPermissionRequirement : IAuthorizationRequirement { }

    public class UserManagementPermissionRequirementHandler : AuthorizationHandler<UserManagementPermissionRequirement>
    {
        private readonly UserDetailsProvider _userDetailsProvider;

        public UserManagementPermissionRequirementHandler(UserDetailsProvider userDetailsProvider)
            => _userDetailsProvider = userDetailsProvider;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserManagementPermissionRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            var routeValues = filterContext?.RouteData?.Values ?? context.Resource as RouteValueDictionary;

            if (int.TryParse(Convert.ToString(routeValues["userRno"]), out var userRno))
            {
                var userMessage = await _userDetailsProvider.DetailsAsync(userRno);

                if (userMessage != null)
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask;
        }
    }
}
