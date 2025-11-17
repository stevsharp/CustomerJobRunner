namespace CustomerSyncConsole;

public interface IStorage
{
    Task SaveAsync(string payloadId, string rawJson, CancellationToken ct);
}
