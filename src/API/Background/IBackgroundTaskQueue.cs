namespace API.Background;

public interface IBackgroundTaskQueue
{
    string Name { get; }
    
    int QueuedCount { get; }

    ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> workItem);

    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}