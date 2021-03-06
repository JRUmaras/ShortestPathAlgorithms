using System.IO;
using System.Threading.Tasks;
using BruteForceSpaService.Options;
using EasyNetQTools.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                        .Configure<BruteForceSpaServiceOptions>(hostBuilderContext.Configuration.GetRequiredSection(nameof(BruteForceSpaServiceOptions)));
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