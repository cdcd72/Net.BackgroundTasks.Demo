namespace API.Background;

public interface IBackgroundTaskQueue
{
    int QueueCount { get; }

    ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> workItem);

    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}