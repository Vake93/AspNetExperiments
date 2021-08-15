using Lib.AspNetCore.ServerTiming;
using Lib.AspNetCore.ServerTiming.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServerTiming();

var app = builder.Build();

app.UseServerTiming();

app.MapGet("/", async (IServerTiming recorder) =>
{
    var delay = Random.Shared.Next(20, 40);
    await Task.Delay(delay);
    recorder.Metrics.Add(new ServerTimingMetric("database", delay));

    delay = Random.Shared.Next(50, 70);
    await Task.Delay(delay);
    recorder.Metrics.Add(new ServerTimingMetric("api-call", delay));

    delay = Random.Shared.Next(100, 110);
    await Task.Delay(delay);
    recorder.Metrics.Add(new ServerTimingMetric("model-processing", delay));

    return "Hello World";
});

app.Run();