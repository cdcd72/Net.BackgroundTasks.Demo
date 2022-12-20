using API.Background;

namespace API.Workers;

public class BackgroundTaskQueueWorker : BackgroundService
{
    private readonly ILogger<BackgroundTaskQueueWorker> _logger;
    private readonly IBackgroundTaskQueue _taskQueue;

    public BackgroundTaskQueueWorker(ILogger<BackgroundTaskQueueWorker> logger, IBackgroundTaskQueue taskQueue)
    {
        _logger = logger;
        _taskQueue = taskQueue;
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