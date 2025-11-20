Read the full article at :

https://coderlegion.com/7559/tutorial-building-a-net-9-console-app-with-hangfire-and-channels

https://dev.to/stevsharp/tutorial-building-a-net-9-console-app-with-hangfire-and-channels-1g02

# CustomerSyncConsole

## Overview
CustomerSyncConsole is a .NET 8 console application that demonstrates:
- Polling simulated data
- Using Channels for message passing
- Triggering Hangfire background jobs when conditions are met
- In-memory storage for simplicity

## Channels
The application uses **System.Threading.Channels** to implement a producer-consumer pattern:
- `DataPollingService` acts as the producer, writing work items into the channel.
- `ChannelConsumerService` acts as the consumer, reading items and processing them.

Channels provide thread-safe, high-performance communication between background tasks without manual locking or complex queue management.

## Hosted Services
The app uses **IHostedService** implementations (`BackgroundService`) to run long-running tasks:
- `DataPollingService` runs periodically to fetch or simulate data.
- `ChannelConsumerService` continuously listens for new items in the channel.

Hosted services integrate seamlessly with the .NET Generic Host, allowing dependency injection, logging, and graceful shutdown.

## Requirements
- .NET 9 SDK
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


## Connect with Me

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Profile-blue)](https://www.linkedin.com/in/spyros-ponaris-913a6937/)
