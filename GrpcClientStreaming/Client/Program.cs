using Grpc.Net.Client;
using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc.Configuration;
using System.Runtime.CompilerServices;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");

var fileService = channel.CreateGrpcService<IFileService>();

Console.WriteLine("Uploading File");

var startTime = DateTime.Now;

var result = await fileService.UploadFileAsync(CreateFileChunksAsync(@"D:\music.mp3"));

var duration = DateTime.Now - startTime;

Console.WriteLine($"Duration: {duration.TotalSeconds} seconds");

Console.ReadLine();

static async IAsyncEnumerable<FileChunk> CreateFileChunksAsync(
    string filePath,
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    using var file = File.OpenRead(filePath);
    var buffer = new byte[8192];

    while (true)
    {
        var read = await file.ReadAsync(buffer, cancellationToken);

        if (read == 0)
        {
            break;
        }

        yield return new FileChunk
        {
            Data = buffer,
        };
    }
}

[Service]
public interface IFileService
{
    [Operation]
    Task<FileUploadResult> UploadFileAsync(
        IAsyncEnumerable<FileChunk> fileChunks,
        CallContext context = default);
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