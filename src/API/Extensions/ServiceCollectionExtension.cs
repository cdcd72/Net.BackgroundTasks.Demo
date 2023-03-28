using API.Background;
using API.Background.Models;

namespace API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBackgroundTaskQueues(this IServiceCollection services, IConfiguration config)
    {
        var taskQueueOptions = config.GetSection(BackgroundTaskQueueOptions.SectionName).Get<BackgroundTaskQueueOptions[]>();

        if (taskQueueOptions is not null)
        {
            foreach (var taskQueueOption in taskQueueOptions)
            {
                services.AddSingleton<IBackgroundTaskQueue>(new BackgroundTaskQueue(taskQueueOption));
            }
        }
        
        return services;
    }
}
