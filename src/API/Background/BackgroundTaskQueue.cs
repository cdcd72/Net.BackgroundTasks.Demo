using System.Threading.Channels;
using API.Background.Models;

namespace API.Background;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    #region Properties

    public string Name { get; }

    public int QueuedCount => _queue.Reader.Count;

    #endregion

    public BackgroundTaskQueue(BackgroundTaskQueueOptions backgroundTaskQueueOptions)
    {
        Name = backgroundTaskQueueOptions.Name;
        
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(new BoundedChannelOptions(backgroundTaskQueueOptions.Capacity)
        {
            FullMode = backgroundTaskQueueOptions.FullMode
        });
    }

    public async ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> workItem)
    {
        if (workItem is null)
            throw new ArgumentNullException(nameof(workItem));

        await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}