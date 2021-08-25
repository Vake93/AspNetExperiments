using Lib.AspNetCore.ServerTiming;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServerTiming();

var app = builder.Build();

app.UseServerTiming();

app.MapGet("/", async (IServerTiming recorder) =>
{
    var delay = Random.Shared.Next(20, 40);
    await recorder.TimeTask(Task.Delay(delay), "database");

    delay = Random.Shared.Next(50, 70);
    await recorder.TimeTask(Task.Delay(delay), "api-call");

    delay = Random.Shared.Next(100, 110);
    await recorder.TimeTask(Task.Delay(delay), "model-processing");

    return "Hello World";
});

app.Run();