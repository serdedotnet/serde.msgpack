using Serde;

namespace Serde.MsgPack;

public static class MsgPackSerializer
{
    public static byte[] Serialize<T>(T value)
        where T : ISerialize<T>
    {
        using var buffer = new ScratchBuffer();
        var writer = new MsgPackWriter(buffer);
        value.Serialize(value, writer);
        return buffer.Span.ToArray();
    }

    public static byte[] Serialize<T, U>(T value, U proxy)
        where U : ISerialize<T>
    {
        using var buffer = new ScratchBuffer();
        var writer = new MsgPackWriter(buffer);
        proxy.Serialize(value, writer);
        return buffer.Span.ToArray();
    }
}
