using c2c_flexiseason.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(c2c_flexiseason.Startup))]
namespace c2c_flexiseason
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<ITicketsService, TicketsService>();
        }
    }
}
