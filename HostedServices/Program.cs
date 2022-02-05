using BruteForceSpaService.Options;
using EasyNetQTools.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace HostedServices
{
    internal static class Program
    {
        private static async Task Main()
        {
            using var host = CreateHostBuilder().Build();
            await host.RunAsync();
        }
        
        private static IHostBuilder CreateHostBuilder()
            => Host
                .CreateDefaultBuilder()
                .ConfigureServices((hostBuilderContext, services) => 
                {
                    services
                        .AddHostedService<BruteForceSpaService.BruteForceSpaService>()
                        .Configure<RabbitMqOptions>(hostBuilderContext.Configuration.GetRequiredSection(nameof(RabbitMqOptions)))
                        .Configure<BruteForceSpaServiceOptions>(hostBuilderContext.Configuration.GetRequiredSection(nameof(BruteForceSpaServiceOptions)))
                        .AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value)
                        .AddSingleton(sp => sp.GetRequiredService<IOptions<BruteForceSpaServiceOptions>>().Value);
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var hostingEnv = hostingContext.HostingEnvironment;
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false)
                        .AddJsonFile($"appsettings.{hostingEnv.EnvironmentName}.json", true);
                });
    }
}