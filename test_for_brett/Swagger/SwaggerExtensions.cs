using Microsoft.OpenApi.Models;

namespace test_for_brett.Swagger

{
    public static class SwaggerExtensions
    {
    private static SwaggerOptions? SwaggerConfig;
        public static IServiceCollection AddSwaggerWithAuthentication(this IServiceCollection services, IConfiguration config, ILogger logger)
        {
        SwaggerConfig = config.Get<SwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
            // this configures swagger to allow developers to drop in an api key when apikey auth is enabled. 
            // see the apikey auth extension for implementation details.
            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "Enter your API Key, prefixed with \"Apikey \"",
                Type = SecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            var key = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                {
                                    { key, new List<string>() }
                                });
            if (SwaggerConfig?.ClientId != null)
            {
                // This configures Swagger to participate in the full authentication flow with the. Change these values to meet your needs. 
                // See https://github.com/domaindrivendev/Swashbuckle.AspNetCore#add-security-definitions-and-requirements for implementation details and other flows. 
                var authUrl = SwaggerConfig.AuthorizationUrl ?? throw new InvalidOperationException("Swagger:AuthorizationUrl is required when specifying Swagger:ClientId");
                var authScope = SwaggerConfig.AuthScope ?? throw new InvalidOperationException("Swagger:AuthScope is required when specifying Swagger:ClientId");
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "Supply a client ID to complete the authentication flow within Swagger.",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri(authUrl),
                            Scopes = { [authScope] = "Default scope" }
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                {
                                    { new OpenApiSecurityScheme
                                        {
                                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
                                        },
                                        Array.Empty<string>()
                                    }
                                });
            }
            else
            {
                logger.LogInformation("Skipping swagger Bearer authentication features.");
            }



            });

            return services;
        }

        // Insert Swagger & its UI into the pipeline, prefill some auth info and title.
        public static WebApplication UseSwaggerWithOptions(this WebApplication app, IConfiguration config)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
            if (SwaggerConfig?.ClientId != null)
            {
                options.OAuthClientId(SwaggerConfig.ClientId);
            }
                options.DocumentTitle = "test_for_brett Swagger UI";

            });
            return app;
        }

    }
}

