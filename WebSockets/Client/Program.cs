using System.Net.WebSockets;

try
{
    var client = new ClientWebSocket();

    await client.ConnectAsync(new Uri("wss://localhost:7077/ws"), CancellationToken.None);

    var data = new byte[1024 * 4];
    var buffer = new ArraySegment<byte>(data);
    using var stream = new MemoryStream(data);

    for (var i = 0; i < 10; i++)
    {
        var message = new Message
        {
            Data = $"Hello, the client time is {DateTime.UtcNow.Ticks} Ticks",
        };

        message.Serialize(stream);

        await client.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);

        var response = await client.ReceiveAsync(buffer, CancellationToken.None);
        message = Message.Deserialize(stream);

        Console.WriteLine(message.Data);
    }

    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Goodbye", CancellationToken.None);
}
catch (Exception e)
{
    Console.WriteLine($"ERROR: {e.GetType()} {e.Message}");
}

Console.WriteLine("Done.");