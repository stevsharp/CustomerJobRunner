using System.Threading.Channels;

using Hangfire;
using Hangfire.MemoryStorage;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CustomerSyncConsole;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Polling:IntervalSeconds"] = "10"
                });
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSimpleConsole(o =>
                {
                    o.TimestampFormat = "HH:mm:ss ";
                    o.SingleLine = true;
                });
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .ConfigureServices((ctx, services) =>
            {
                services.AddHttpClient();
                services.AddSingleton(Channel.CreateUnbounded<CustomerWorkItem>());
                services.AddHangfire(config => config.UseMemoryStorage());
                services.AddHangfireServer();
                services.AddSingleton<ICustomerJobs, CustomerJobs>();
                services.AddSingleton<IStorage, InMemoryStorage>();
                services.AddHostedService<DataPollingService>();
                services.AddHostedService<ChannelConsumerService>();
            })
            .Build();

        await host.RunAsync();
    }
}
