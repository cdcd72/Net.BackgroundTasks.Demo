using API.Background;
using API.Extensions;

namespace API.Workers;

public class BackgroundTaskQueueWorker : BackgroundService
{
    private readonly ILogger<BackgroundTaskQueueWorker> _logger;
    private readonly IBackgroundTaskQueue _taskQueue;

    public BackgroundTaskQueueWorker(ILogger<BackgroundTaskQueueWorker> logger, IEnumerable<IBackgroundTaskQueue> taskQueues)
    {
        _logger = logger;
        _taskQueue = taskQueues.GetBackgroundTaskQueue("Demo");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background Queue Worker is running.");

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
                _logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background Queue Worker is stopping.");

        await base.StopAsync(stoppingToken);
    }
}