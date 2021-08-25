var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/new", () =>
{
    Log.SayHello(app.Logger, "World");
    return "Hello World!";
});

app.MapGet("/old", () =>
{
    app.Logger.LogInformation("Hello {name}", "World");
    return "Hello World!";
});

app.Run();

public static partial class Log
{
    [LoggerMessage(0, LogLevel.Information, "Hello {name}")]
    public static partial void SayHello(ILogger logger, string name);
}

class Foo { }