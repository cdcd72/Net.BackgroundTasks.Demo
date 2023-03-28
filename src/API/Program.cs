using API.Extensions;
using API.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add background queues
builder.Services.AddBackgroundTaskQueues(builder.Configuration);

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
