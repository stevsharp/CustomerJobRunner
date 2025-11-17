using Microsoft.Extensions.Logging;

namespace CustomerSyncConsole;

public sealed class InMemoryStorage(ILogger<InMemoryStorage> logger) : IStorage
{
    private readonly ILogger<InMemoryStorage> _logger = logger;
    private readonly Dictionary<string, string> _store = new();

    public Task SaveAsync(string payloadId, string rawJson, CancellationToken ct)
    {
        _store[payloadId] = rawJson;
        _logger.LogInformation("Saved payload {PayloadId} in-memory (size={Size}).", payloadId, _store.Count);
        return Task.CompletedTask;
    }
}
