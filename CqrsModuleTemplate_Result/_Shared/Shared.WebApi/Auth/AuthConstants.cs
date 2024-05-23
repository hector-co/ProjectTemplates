namespace Shared.WebApi.Auth;

public static class AuthConstants
{
    public static class Claims
    {
        public const string CustomClaimPrefix = "custom:";

        public const string Role = "role";
        public const string Subject = "sub";
        public const string Name = "name";
        public const string GivenName = "given_name";
        public const string FamilyName = "family_name";
        public const string SurName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        public const string Email = "email";
        public const string Issuer = "iss";
        public const string Audience = "aud";
    }
}
