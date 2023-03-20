using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Types;
using GraphQL;
using System.Security.Claims;

namespace BasicAuthGraphQL
{
    class MyMiddleware<TSchema> : GraphQLHttpMiddleware<TSchema>
        where TSchema : ISchema
    {
        public MyMiddleware(
            RequestDelegate next,
            IGraphQLTextSerializer serializer,
            IDocumentExecuter<TSchema> documentExecuter,
            IServiceScopeFactory serviceScopeFactory,
            GraphQLHttpMiddlewareOptions options,
            IHostApplicationLifetime hostApplicationLifetime)
            : base(next, serializer, documentExecuter, serviceScopeFactory, options, hostApplicationLifetime)
        {

        }

        protected override string SelectResponseContentType(HttpContext context)
            => "application/json";

        /// <summary>
        /// Perform authentication, if required, and return <see langword="true"/> if the
        /// request was handled (typically by returning an error message).  If <see langword="false"/>
        /// is returned, the request is processed normally.
        /// </summary>
        //protected override async ValueTask<bool> HandleAuthorizeAsync(HttpContext context, RequestDelegate next)
        //{
        //    var claimName = new Claim(ClaimTypes.Name, "SAlexander");
        //    var claimPermissions = new Claim("Permissions", "read");
        //    var identity = new ClaimsIdentity(new[] { claimName, claimPermissions }, "BasicAuthentication"); // this uses basic auth
        //    var principal = new ClaimsPrincipal(identity);
        //    context.User = principal;
        //    await next.Invoke(context);
        //    return true;
        //}

    }
}
