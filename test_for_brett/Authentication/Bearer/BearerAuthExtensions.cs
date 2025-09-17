using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel;

namespace test_for_brett.Authentication;

public static class BearerAuthExtensions
{
    public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder, IConfiguration config)
    {
        return builder.AddJwtBearer(options =>
        {
            config.GetSection("Bearer").Bind(options);
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                RequireAudience = true,
                RequireExpirationTime = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = true
            };
            options.Validate();
        });
    }
}
