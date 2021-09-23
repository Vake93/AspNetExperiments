using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<WebSocketHandler>();

var app = builder.Build();
app.UseWebSockets();
app.MapGet("/", () => "Hello World!");
app.Map("/ws", a => a.UseMiddleware<WebSocketHandler>());

app.Run();

class WebSocketHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status505HttpVersionNotsupported;
            await context.Response.WriteAsync("Only WebSocket requests are allowed at this endpoint.");
            await context.Response.CompleteAsync();
            return;
        }

        var webSocket = await context.WebSockets.AcceptWebSocketAsync();

        var data = new byte[1024 * 4];
        var buffer = new ArraySegment<byte>(data);
        using var stream = new MemoryStream(data);

        do
        {
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            var message = Message.Deserialize(stream);
            message.Data = $"Hello, the server time is {DateTime.UtcNow.Ticks} Ticks";
            message.Serialize(stream);
            await webSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);

        } while (!webSocket.CloseStatus.HasValue);

        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, webSocket.CloseStatusDescription, CancellationToken.None);
    }
}