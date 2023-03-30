using API.Background;

namespace API.Extensions;

public static class BackgroundTaskQueueExtension
{
    public static IBackgroundTaskQueue GetBackgroundTaskQueue(this IEnumerable<IBackgroundTaskQueue> queues, string name) => queues.First(queue => queue.Name == name);
}