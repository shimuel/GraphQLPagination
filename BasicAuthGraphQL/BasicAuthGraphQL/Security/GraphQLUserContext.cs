using System.Security.Claims;

namespace BasicAuthGraphQL.Security
{
    public class GraphQLUserContext : Dictionary<string, object?>
    {
        public ClaimsPrincipal User { get; }

        public GraphQLUserContext(HttpContext context)
        {
            User = context.User;
        }
    }
}
