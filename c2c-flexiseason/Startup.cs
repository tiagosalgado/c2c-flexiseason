using c2c_flexiseason.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

[assembly: FunctionsStartup(typeof(c2c_flexiseason.Startup))]
namespace c2c_flexiseason
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("AppConfigurationConnectionString"));
            var config = configurationBuilder.Build();

            builder.Services.AddOptions();

            var _settings = new Settings
            {
                ApiSettings = new ApiSettings
                {
                    BaseUrl = config["BaseUrl"],
                    Username = config["c2c_email"],
                    Password = config["c2c_pwd"]
                },
                SmartcardInfo = new SmartcardInfo
                {
                    SerialNumber = config["SmartcardSerialNumber"]
                }
            };

            builder.Services.AddTransient<ITicketsService>(_ => new TicketsService(Options.Create(_settings)));
        }
    }
}
