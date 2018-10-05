using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Core;
using Microsoft.AspNetCore.Authorization;

namespace DemoBlog.Authorization
{
    public class ManageArticleAuthorizationRequirement: IAuthorizationRequirement
    {

    }

    public class ManageArticleAuthorizationHandler : AuthorizationHandler<ManageArticleAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageArticleAuthorizationRequirement requirement, string roleName)
        {
            if (context.User == null)
                return Task.CompletedTask;

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageArtiles) || context.User.IsInRole(roleName))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
