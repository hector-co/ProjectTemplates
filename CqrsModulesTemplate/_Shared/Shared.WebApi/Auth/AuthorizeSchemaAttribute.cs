using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace Shared.WebApi.Auth;

public class AuthorizeSchemaAttribute : AuthorizeAttribute
{
    public AuthorizeSchemaAttribute() : base()
    {
        AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    }
}