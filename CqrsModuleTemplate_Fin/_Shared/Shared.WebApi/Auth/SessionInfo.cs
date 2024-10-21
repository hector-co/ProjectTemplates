using Microsoft.AspNetCore.Http;
using Shared.Application.Auth;
using System.Security.Claims;

namespace Shared.WebApi.Auth;

public class SessionInfo : ISessionInfo
{
    private IEnumerable<string> _roles = new List<string>();
    private readonly Dictionary<string, string> _meta = new(StringComparer.InvariantCultureIgnoreCase);

    public string UserId { get; private set; } = string.Empty;
    public IEnumerable<string> Roles => _roles.ToList();
    public bool IsAuthenticated => string.IsNullOrEmpty(UserId);

    public string GivenName { get; private set; } = string.Empty;

    public string FamilyName { get; private set; } = string.Empty;

    public string Name => $"{GivenName} {FamilyName}";

    public string Email { get; private set; } = string.Empty;
    public string Issuer { get; private set; } = string.Empty;

    public IReadOnlyDictionary<string, string> Meta => _meta.AsReadOnly();

    public bool IsInRole(string role)
    {
        return _roles.Contains(role);
    }

    internal async Task SetData(HttpContext context)
    {
        if (!context.User.Identity?.IsAuthenticated ?? false) return;

        _roles = GetClaimValues(context, AuthConstants.Claims.Role, ClaimTypes.Role).ToList();
        GivenName = GetClaimValues(context, AuthConstants.Claims.GivenName, ClaimTypes.GivenName).FirstOrDefault() ?? "";
        FamilyName = GetClaimValues(context, AuthConstants.Claims.FamilyName, AuthConstants.Claims.SurName).FirstOrDefault() ?? "";
        Email = GetClaimValues(context, AuthConstants.Claims.Email, ClaimTypes.Email).FirstOrDefault() ?? "";
        UserId = GetClaimValues(context, AuthConstants.Claims.Subject, ClaimTypes.NameIdentifier).FirstOrDefault() ?? "";
        Issuer = GetClaimValues(context, AuthConstants.Claims.Issuer).FirstOrDefault() ?? "";

        LoadCustomClaims(context.User);

        await Task.CompletedTask;
    }

    private void LoadCustomClaims(ClaimsPrincipal principal)
    {
        foreach (var cc in principal.Claims
            .Where(c => c.Type.StartsWith(AuthConstants.Claims.CustomClaimPrefix, StringComparison.InvariantCultureIgnoreCase)))
        {
            var key = cc.Type.Split(':')[1];
            _meta.Add(key, cc.Value);
        }
    }

    private static IEnumerable<string> GetClaimValues(HttpContext context, params string[] claimTypes)
    {
        var claims = context.User.Claims.Where(c => claimTypes.Contains(c.Type, StringComparer.InvariantCultureIgnoreCase));

        if (!claims.Any())
            yield break;

        foreach (var claim in claims)
            yield return claim.Value ?? "";
    }
}
