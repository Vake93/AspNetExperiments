var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHelloService, HelloService>();

var app = builder.Build();

app.MapGet("/", (IHelloService helloService) => helloService.HelloMessage);

app.Run();

public interface IHelloService
{
    string HelloMessage { get; }
}

public class HelloService : IHelloService
{
    public string HelloMessage => "Hello World";
}