using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;

namespace BasicAuthGraphQL.Security
{

    public class ReadRequirement : IAuthorizationRequirement
    {
        public string Access => Constants.POLICY_READ;
    }

    public class WriteRequirement : IAuthorizationRequirement
    {
        public string Access => Constants.POLICY_UPDATE;
    }

    public class UIRequirement : IAuthorizationRequirement
    {
        public string Access => Constants.POLICY_UI;
    }

    public class Constants
    {
        public const string POLICY_READ = "read";
        public const string POLICY_UPDATE = "update";
        public const string POLICY_UI = "ui";
        public const string CLAIM_PERMISSIONS = "permissions";
    }

    //public class SecurityPolicy
    //{
    //    [JsonPropertyName(Constants.POLICY_READ)]
    //    public bool CanRead { get; set; }

    //    [JsonPropertyName(Constants.POLICY_UPDATE)]
    //    public bool CanUpdate { get; set; }
    //}
}
