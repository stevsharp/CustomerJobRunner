using System.Threading.Channels;

using Hangfire;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CustomerSyncConsole;

public sealed class ChannelConsumerService(
    Channel<CustomerWorkItem> channel,
    IBackgroundJobClient backgroundJobClient,
    ILogger<ChannelConsumerService> logger) : BackgroundService
{
    private readonly ChannelReader<CustomerWorkItem> _reader = channel.Reader;
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
    private readonly ILogger<ChannelConsumerService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Channel consumer started.");

        await foreach (var item in _reader.ReadAllAsync(stoppingToken))
        {
            if (item.CustomerId == 1)
            {
                var jobId = _backgroundJobClient.Enqueue<ICustomerJobs>(job => job.ProcessCustomer(item.CustomerId, item.PayloadId, CancellationToken.None));
                _logger.LogInformation("Enqueued Hangfire job {JobId} for Customer {CustomerId}, Payload {PayloadId}", jobId, item.CustomerId, item.PayloadId);
            }
        }
        _logger.LogInformation("Channel consumer stopping.");
    }
}
