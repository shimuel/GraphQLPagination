using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BasicAuthGraphQL.Security
{
    public class RequirementAuthorizationHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            //Filter specific claim    
            var _ = (context.User.Claims?.FirstOrDefault(
                x => x.Type.Equals(Constants.CLAIM_PERMISSIONS, StringComparison.OrdinalIgnoreCase))?.Value);

            if (string.IsNullOrEmpty(_))
            {
                //context.Fail(new AuthorizationFailureReason(this,"Access denied !!!"));
                return Task.CompletedTask;
            }

            var permissions = _.Split(',').ToList();
            foreach (var requirement in pendingRequirements)
            {
                if (requirement is ReadRequirement)
                {
                    if (HasAccess(context.User, context.Resource, Constants.POLICY_READ, permissions))
                    {
                        context.Succeed(requirement);
                    }
                }
                else if (requirement is WriteRequirement)
                {
                    if (HasAccess(context.User, context.Resource, Constants.POLICY_UPDATE, permissions))
                    {
                        context.Succeed(requirement);
                    }
                }
                else if (requirement is UIRequirement )
                {
                    if (HasAccess(context.User, context.Resource, Constants.POLICY_UI, permissions))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }

        private static bool HasAccess(ClaimsPrincipal user, object? resource, string perm, List<string> permissions)
        {
            return permissions.Contains(perm);
        }

    }
}
