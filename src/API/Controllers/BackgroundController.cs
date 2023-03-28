using API.Background;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class BackgroundController : ControllerBase
{
    private readonly ILogger<BackgroundController> _logger;
    private readonly IBackgroundTaskQueue _taskQueue;

    public BackgroundController(ILogger<BackgroundController> logger, IEnumerable<IBackgroundTaskQueue> taskQueues)
    {
        _logger = logger;
        _taskQueue = taskQueues.First(queue => queue.Name == "Demo");
    }
    
    [HttpGet]
    [Route(nameof(GetQueueCount))]
    public int GetQueueCount() => _taskQueue.QueuedCount;

    [HttpPost]
    [Route(nameof(Enqueue))]
    public async Task<string> Enqueue(string value)
    {
        await _taskQueue.EnqueueAsync(token => BuildCustomWorkItem(value, token));
        
        return value;
    }

    #region Private Method

    private async ValueTask BuildCustomWorkItem(string value, CancellationToken token)
    {
        // Simulate one 5-second task to complete
        // for each enqueued work item
        
        token.ThrowIfCancellationRequested();
        
        var guid = $"{Guid.NewGuid()}";

        _logger.LogInformation("Queued Background Task {Guid} is starting.", guid);

        try
        {
            _logger.LogInformation("Queued Background Task {Guid} is running with value {Value}.", guid, value);
                
            await Task.Delay(TimeSpan.FromSeconds(5), token);
        }
        catch (OperationCanceledException)
        {
            // Prevent throwing if the Delay is cancelled
        }

        _logger.LogInformation("Queued Background Task {Guid} is complete.", guid);
    }

    #endregion
}
