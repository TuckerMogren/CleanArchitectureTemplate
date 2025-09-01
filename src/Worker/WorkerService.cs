using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a background service that periodically logs the current time.
/// </summary>
/// <param name="logger">The logger used to log information messages.</param>
public sealed class WorkerService(ILogger<WorkerService> logger) : BackgroundService
{
    /// <summary>
    /// Executes the background service.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}

