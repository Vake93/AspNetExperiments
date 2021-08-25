using ProtoBuf;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCodeFirstGrpc();

var app = builder.Build();

app.MapGrpcService<FileService>();
app.MapGet("/", () => "Hello from File Service!");

app.Run();

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public async Task<FileUploadResult> UploadFileAsync(
        IAsyncEnumerable<FileChunk> fileChunks,
        CancellationToken cancellationToken = default)
    {
        // Saving the file to temp folder for demo
        var path = Path.GetTempFileName();

        using (var file = File.OpenWrite(path))
        {
            await foreach (var fileChunk in fileChunks)
            {
                await file.WriteAsync(fileChunk.Data, cancellationToken);
            }

            await file.FlushAsync(cancellationToken);
        }

        _logger.LogInformation("Saved uploaded file at: {path}", path);

        return new FileUploadResult
        {
            Success = true,
        };
    }
}

[Service]
public interface IFileService
{
    [Operation]
    Task<FileUploadResult> UploadFileAsync(
        IAsyncEnumerable<FileChunk> fileChunks,
        CancellationToken cancellationToken = default);
}

[ProtoContract]
public class FileChunk
{
    [ProtoMember(1)]
    public byte[] Data { get; init; } = Array.Empty<byte>();
}

[ProtoContract]
public class FileUploadResult
{
    [ProtoMember(1)]
    public bool Success { get; init; }
}
