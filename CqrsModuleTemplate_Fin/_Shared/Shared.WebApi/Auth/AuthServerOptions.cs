namespace Shared.WebApi.Auth;

public class AuthServerOptions
{
    public const string AuthServer = nameof(AuthServer);

    public string ServerUrl { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientPassword { get; set; } = string.Empty;
}
