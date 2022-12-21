using API.Background;
using API.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add background queue
builder.Services.AddSingleton<IBackgroundTaskQueue>(_ =>
{
    if (!int.TryParse(builder.Configuration["BackgroundTask:QueueCapacity"], out var queueCapacity)) queueCapacity = 100;
    return new BackgroundTaskQueue(queueCapacity);
});
// Add background queue worker
builder.Services.AddHostedService<BackgroundTaskQueueWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
