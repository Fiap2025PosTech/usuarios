using Microsoft.AspNetCore.Authorization;
using Usuarios.Domain.Enums;

namespace Usuarios.API.Authorization;

public static class AuthorizationOptionsExtensions
{
    public static AuthorizationOptions AddPolicyWithPermission(this AuthorizationOptions options, string policyName, AccessLevel accessLevel)
    {
        options.AddPolicy(policyName, policy => policy.Requirements.Add(new RolesRequirement(accessLevel)));
        return options;
    }
}