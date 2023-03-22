using System.Security.Claims;
using GraphQL;
using GraphQL.Server.Transports.AspNetCore.WebSockets;
using GraphQL.Transport;
using GraphQL.Types.Relay.DataObjects;

namespace BasicAuthGraphQL
{
    public class MyAuthService : IWebSocketAuthenticationService
    {
        private readonly IGraphQLSerializer _serializer;

        public MyAuthService(IGraphQLSerializer serializer)
        {
            _serializer = serializer;
        }

        public Task AuthenticateAsync(IWebSocketConnection connection, string subProtocol, OperationMessage operationMessage)
        {
            // read payload of ConnectionInit message and look for an "Authorization" entry that starts with "Bearer "
            var payload = _serializer.ReadNode<Inputs>(operationMessage.Payload);
            if ((payload?.TryGetValue("Authorization", out var value) ?? false) && value is string valueString)
            {
                var user = ParseToken(valueString);
                if (user != null)
                {
                    // set user and indicate authentication was successful
                    connection.HttpContext.User = user;
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);  // authentication failed
        }

        private ClaimsPrincipal? ParseToken(string authorizationHeaderValue)
        {
            // parse header value and return user, or null if unable
            return null;
        }
    }
}


///https://github.com/graphql-dotnet/server
///Authentication for WebSocket requests
// Since WebSocket requests from browsers cannot typically carry a HTTP Authorization header,
// you will need to authorize requests via the ConnectionInit WebSocket message or carry the authorization token within the URL. Below is a sample of the former:
///
/// To authorize based on information within the query string, it is recommended to derive from GraphQLHttpMiddleware<T> and override InvokeAsync,
/// setting HttpContext.User based on the query string parameters, and then calling base.InvokeAsync.
/// Alternatively you may override HandleAuthorizeAsync which will execute for GET/POST requests, and HandleAuthorizeWebSocketConnectionAsync for WebSocket requests.
/// Note that InvokeAsync will execute even if the protocol is disabled in the options via disabling HandleGet or similar; HandleAuthorizeAsync and HandleAuthorizeWebSocketConnectionAsync will not.