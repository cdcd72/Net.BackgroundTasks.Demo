using API.Background;
using API.Extensions;

namespace API.Workers;

public class BackgroundTaskQueueWorker(ILogger<BackgroundTaskQueueWorker> logger, IEnumerable<IBackgroundTaskQueue> taskQueues) : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue = taskQueues.GetBackgroundTaskQueue("Demo");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Background Queue Worker is running.");

        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(stoppingToken);

            try
            {
                await workItem(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Background Queue Worker is stopping.");

        await base.StopAsync(stoppingToken);
    }
}