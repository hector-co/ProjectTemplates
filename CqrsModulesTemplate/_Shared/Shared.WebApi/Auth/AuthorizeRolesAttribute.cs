namespace Shared.WebApi.Auth;

public class AuthorizeRolesAttribute : AuthorizeSchemaAttribute
{
    public AuthorizeRolesAttribute(params string[] roles) : base()
    {
        if (roles != null && roles.Length > 0)
            Roles = string.Join(",", roles);
    }
}