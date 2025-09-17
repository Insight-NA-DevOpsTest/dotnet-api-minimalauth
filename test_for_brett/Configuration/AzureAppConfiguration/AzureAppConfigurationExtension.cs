using System.Reflection;

namespace test_for_brett.Configuration
{
    public static class AzureAppConfigurationExtension
    {
        public const string ConfigSection = "Configuration:AzureAppConfigConnectionString";
        public static void UseAzureAppConfiguration(this WebApplicationBuilder builder, ILogger logger)
        {
            try
            {
                var config = builder.Configuration;
                var environment = builder.Environment.EnvironmentName.ToLower();

                config
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{environment}.json", true, true);

                if (environment == "development")
                {
                    config.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
                }

                var appConfigConnectionString = config.GetSection(ConfigSection);

                if (!string.IsNullOrWhiteSpace(appConfigConnectionString?.Value))
                {
                    config.AddAzureAppConfiguration(options =>
                    {
                        options.Connect(appConfigConnectionString.Value);
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ConfigurationException($"Error in reading app configuration. Message: {ex.Message}", ex);
            }
        }
    }
}