using System.Threading.Channels;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CustomerSyncConsole;

public sealed class DataPollingService(ILogger<DataPollingService> logger,
    Channel<CustomerWorkItem> channel, IConfiguration config,IStorage storage) : BackgroundService
{
    /// <summary>
    /// 
    /// </summary>
    private readonly ILogger<DataPollingService> _logger = logger;
    private readonly ChannelWriter<CustomerWorkItem> _writer = channel.Writer;
    private readonly IConfiguration _config = config;
    private readonly IStorage _storage = storage;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalSeconds = _config.GetValue("Polling:IntervalSeconds", 10);

        var timer = new PeriodicTimer(TimeSpan.FromSeconds(intervalSeconds));

        _logger.LogInformation("Polling started. Interval={Interval}s", intervalSeconds);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var simulated = new[]
                {
                    new IncomingDto { CustomerId = 1, PayloadId = Guid.NewGuid().ToString("N") },
                    new IncomingDto { CustomerId = 2, PayloadId = Guid.NewGuid().ToString("N") },
                };

                foreach (var dto in simulated)
                {
                    var raw = System.Text.Json.JsonSerializer.Serialize(dto);

                    await _storage.SaveAsync(dto.PayloadId, raw, stoppingToken);

                    var item = new CustomerWorkItem(dto.CustomerId, dto.PayloadId, DateTimeOffset.UtcNow, raw);
                    
                    await _writer.WriteAsync(item, stoppingToken);

                    _logger.LogInformation("Queued work item for Customer {CustomerId}, Payload {PayloadId}", dto.CustomerId, dto.PayloadId);
                }
            }
        }
        finally
        {
            _writer.TryComplete();
        }
    }

    private sealed class IncomingDto
    {
        public int CustomerId { get; set; }
        public string PayloadId { get; set; } = Guid.NewGuid().ToString("N");
    }
}
