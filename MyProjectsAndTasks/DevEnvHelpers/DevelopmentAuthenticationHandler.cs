using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MyProjectsAndTasks.DevEnvHelpers;

/// <summary>
/// This class is to allow us to easily test the authorization policies in development without having to set up a full authentication mechanism.
/// THis should not be possible in production, and should only be used for development purposes. It creates a fake user with the "Admin" role, which allows us to test the authorization policies that require the "Admin" role. You can change the role to "User" to test the policies that require the "User" role.
/// </summary>
/// <param name="options"></param>
/// <param name="logger"></param>
/// <param name="encoder"></param>
public class DevelopmentAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Claim[] claims = [
            new (ClaimTypes.Name, "DevUser"),
            new (ClaimTypes.Role, "Admin") // Change "Admin" to "User" here to test the different policies!
        ];
        ClaimsIdentity identity = new(claims, "DevScheme");
        ClaimsPrincipal principal = new(identity);
        AuthenticationTicket ticket = new(principal, "DevScheme");

        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }
}