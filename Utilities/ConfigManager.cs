// ...existing code...
// Utility for reading configuration from appsettings.json
using Microsoft.Extensions.Configuration;

namespace selenium_xunit_reqnroll_framework.Utilities
{
    public static class ConfigManager
    {
        private static IConfigurationRoot config;
        static ConfigManager()
        {
            config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
        public static string BaseUrl => config["BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
        public static string Browser => config["Browser"] ?? throw new InvalidOperationException("Browser is not configured.");
        public static int Timeout
        {
            get
            {
                var timeoutValue = config["Timeout"];
                if (string.IsNullOrWhiteSpace(timeoutValue))
                    throw new InvalidOperationException("Timeout is not configured.");
                return int.Parse(timeoutValue);
            }
        }
    }
}
