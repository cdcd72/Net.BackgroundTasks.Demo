using System.Threading.Channels;
using API.Background.Models;

namespace API.Background;

public class BackgroundTaskQueue(BackgroundTaskQueueOptions backgroundTaskQueueOptions) : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue = 
        Channel.CreateBounded<Func<CancellationToken, ValueTask>>(new BoundedChannelOptions(backgroundTaskQueueOptions.Capacity)
        {
            FullMode = backgroundTaskQueueOptions.FullMode
        });

    #region Properties

    public string Name { get; } = backgroundTaskQueueOptions.Name;

    public int QueuedCount => _queue.Reader.Count;

    #endregion

    public async ValueTask EnqueueAsync(Func<CancellationToken, ValueTask> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}