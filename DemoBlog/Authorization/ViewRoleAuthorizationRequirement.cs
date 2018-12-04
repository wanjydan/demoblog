using System.Threading.Tasks;
using DAL.Core;
using Microsoft.AspNetCore.Authorization;

namespace DemoBlog.Authorization
{
    public class ViewRoleAuthorizationRequirement : IAuthorizationRequirement
    {
    }


    public class ViewRoleAuthorizationHandler : AuthorizationHandler<ViewRoleAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ViewRoleAuthorizationRequirement requirement, string roleName)
        {
            if (context.User == null)
                return Task.CompletedTask;

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewRoles) ||
                context.User.IsInRole(roleName))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}