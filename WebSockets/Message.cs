using ProtoBuf;

[ProtoContract]
public class Message
{
    [ProtoMember(1)]
    public string Data { get; set; } = string.Empty;

    public void Serialize(Stream stream)
    {
        stream.Position = 0;
        Serializer.SerializeWithLengthPrefix(stream, this, PrefixStyle.Fixed32);
    }

    public static Message Deserialize(Stream stream)
    {
        stream.Position = 0;
        return Serializer.DeserializeWithLengthPrefix<Message>(stream, PrefixStyle.Fixed32);
    }
}