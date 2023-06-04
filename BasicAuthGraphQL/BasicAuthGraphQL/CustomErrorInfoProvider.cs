
using GraphQL.Server.Transports.AspNetCore.Errors;
using GraphQL;
using System.Text;
using BasicAuthGraphQL.Security;
using GraphQL.Execution;
using Microsoft.AspNetCore.Authorization;

namespace BasicAuthGraphQL
{
    /// <summary>
    /// Custom <see cref="ErrorInfoProvider"/> implementing a dedicated error message for the sample <see cref="IAuthorizationRequirement"/>
    /// used in this MS article: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies
    /// </summary>
    public class CustomErrorInfoProvider : ErrorInfoProvider
    {
        public override ErrorInfo GetInfo(ExecutionError executionError)
        {
            var info = base.GetInfo(executionError);

            if (executionError is AccessDeniedError accessDeniedError)
                info.Message = GetAuthorizationErrorMessage(accessDeniedError);

            return info;
        }

        private static string GetAuthorizationErrorMessage(AccessDeniedError error)
        {
            var errorMessage = new StringBuilder();
            errorMessage.Append(error.Message);

            if (error.PolicyAuthorizationResult != null)
            {
                foreach (var failedRequirement in error.PolicyAuthorizationResult.Failure!
                             .FailedRequirements)
                {
                    switch (failedRequirement)
                    {
                        case UIRequirement uiRequirement:
                            errorMessage.AppendLine();
                            errorMessage.Append($"no ui Access --> request denied !!!");
                            errorMessage.AppendLine();
                            break;
                        case ReadRequirement readRequirement:
                            errorMessage.AppendLine();
                            errorMessage.Append($"no read Access --> request denied !!!");
                            errorMessage.AppendLine();
                            break;
                        case WriteRequirement updateRequirement:
                            errorMessage.AppendLine();
                            errorMessage.Append($"no write Access --> request denied !!!");
                            errorMessage.AppendLine();
                            break;
                        case SubscribeRequirement subscribeRequirement:
                            errorMessage.AppendLine();
                            errorMessage.Append($"no subscription Access --> request denied !!!");
                            errorMessage.AppendLine();
                            break;
                    }
                }
            }

            return errorMessage.ToString();
        }
    }
}