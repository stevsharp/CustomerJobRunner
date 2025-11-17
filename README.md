# CustomerSyncConsole

## Overview
CustomerSyncConsole is a .NET 8 console application that demonstrates:
- Polling simulated data
- Using Channels for message passing
- Triggering Hangfire background jobs when conditions are met
- In-memory storage for simplicity

## Requirements
- .NET 8 SDK
- NuGet packages:
  - Hangfire
  - Hangfire.MemoryStorage
  - Microsoft.Extensions.Hosting
  - Microsoft.Extensions.Logging.Console

## How to Run
```bash
dotnet restore
dotnet run
```

## How It Works
- `DataPollingService` simulates fetching data and writes work items to a channel.
- `ChannelConsumerService` reads from the channel and enqueues Hangfire jobs for CustomerId = 1.
- Hangfire processes jobs in-memory.

