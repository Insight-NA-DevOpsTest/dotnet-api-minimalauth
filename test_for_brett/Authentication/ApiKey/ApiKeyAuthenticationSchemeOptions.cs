using Microsoft.AspNetCore.Authentication;

namespace test_for_brett.Authentication;

public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    /// <summary>
    ///     The key user must provide in Authorization header
    /// </summary>
    public string? Secret { get; set; }

    /// <summary>
    ///     The header which is being checked for valid key, by default is Authorization
    /// </summary>
    public string Header { get; set; } = "Authorization";
}
