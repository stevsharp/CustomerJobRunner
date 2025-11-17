namespace CustomerSyncConsole;

public interface ICustomerJobs
{
    Task ProcessCustomer(int customerId, string payloadId, CancellationToken ct = default);
}
