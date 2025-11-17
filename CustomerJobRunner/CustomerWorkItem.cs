namespace CustomerSyncConsole;

public sealed record CustomerWorkItem(int CustomerId, string PayloadId, DateTimeOffset ReceivedAt, string RawJson);
