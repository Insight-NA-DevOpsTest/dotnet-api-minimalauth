using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace test_for_brett.Authentication;

/// <summary>
///     Handles ApiKey authentication scheme
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
{
    /// <summary>
    ///     Initializes a new instance of ApiKeyAuthenticationHandler
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="encoder"></param>
    /// <param name="clock"></param>
    public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    /// <summary>
    ///     Handles authentication by checking if there is proper api key set in HTTP Authorization header
    /// </summary>
    /// <returns><see cref="AuthenticateResult"/></returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult returnValue = AuthenticateResult.NoResult();

        if (Context.Request.Headers[Options.Header].Count == 0)
        {
            returnValue = AuthenticateResult.NoResult();
        }
        else
        {
            string schemePrefix = ApiKeyAuthExtensions.Scheme + " ";
            string? headerValue = Context.Request.Headers[Options.Header][0];

            if (headerValue?.StartsWith(schemePrefix) ?? false)
            {

                string apiKey = headerValue.Substring(schemePrefix.Length);

                if (apiKey == Options.Secret)
                {
                    var identity = new ClaimsIdentity(ApiKeyAuthExtensions.Scheme);
                    var principal = new ClaimsPrincipal(identity);
                    var authenticationTicket = new AuthenticationTicket(principal, ApiKeyAuthExtensions.Scheme);

                    Context.User.AddIdentity(identity);

                    returnValue = AuthenticateResult.Success(authenticationTicket);
                }
            }
        }
        return await Task.FromResult(returnValue);
    }
}