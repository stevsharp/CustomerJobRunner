using Microsoft.Extensions.Logging;

namespace CustomerSyncConsole;

public sealed class CustomerJobs(ILogger<CustomerJobs> logger) : ICustomerJobs
{
    private readonly ILogger<CustomerJobs> _logger = logger;

    public async Task ProcessCustomer(int customerId, string payloadId, CancellationToken ct = default)
    {
        _logger.LogInformation("Hangfire job started for Customer {CustomerId}, Payload {PayloadId}", customerId, payloadId);

        await Task.Delay(TimeSpan.FromSeconds(2), ct);

        _logger.LogInformation("Hangfire job completed for Customer {CustomerId}, Payload {PayloadId}", customerId, payloadId);
    }
}
