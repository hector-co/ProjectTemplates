namespace Shared.Application.Auth;

public interface ISessionInfo
{
    bool IsAuthenticated { get; }
    IEnumerable<string> Roles { get; }
    string UserId { get; }
    string GivenName { get; }
    string FamilyName { get; }
    string Name { get; }
    string Email { get; }
    string Issuer { get; }
    bool IsInRole(string role);
    IReadOnlyDictionary<string, string> Meta { get; }
}