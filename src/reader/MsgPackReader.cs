

using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using Serde.IO;

namespace Serde.MsgPack;

internal sealed partial class MsgPackReader<TReader> : IDeserializer
    where TReader : IBufReader
{
    private TReader _reader;

    public MsgPackReader(TReader reader)
    {
        _reader = reader;
        // start with a filled buffer
        _reader.FillBuffer(1);
    }

    void IDisposable.Dispose()
    { }

    [DoesNotReturn]
    private static void ThrowEof()
    {
        throw new Exception("Unexpected end of stream");
    }

    T IDeserializer.ReadAny<T>(IDeserializeVisitor<T> v)
    {
        throw new NotImplementedException();
    }

    bool IDeserializer.ReadBool() => ReadBool();

    private bool ReadBool()
    {
        var b = EatByteOrThrow();
        if (b == 0xc2)
        {
            return false;
        }
        if (b == 0xc3)
        {
            return true;
        }
        throw new Exception($"Expected boolean, got 0x{b:x}");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private ReadOnlySpan<byte> RefillNoEof(int fillCount)
    {
        if (!_reader.FillBuffer(fillCount))
        {
            ThrowEof();
        }
        return _reader.Span;
    }

    private byte PeekByteOrThrow()
    {
        var span = _reader.Span;
        if (span.Length == 0)
        {
            span = RefillNoEof(1);
        }
        return span[0];
    }

    private byte EatByteOrThrow()
    {
        var result = PeekByteOrThrow();
        _reader.Advance(1);
        return result;
    }

    /// <summary>
    /// Eats at least one byte from the buffer.
    /// </summary>
    bool TryReadByte(out byte result)
    {
        return TryReadByte(EatByteOrThrow(), out result);
    }

    bool TryReadByte(byte b, out byte result)
    {
        if (b <= 0x7f)
        {
            result = b;
            return true;
        }
        if (b == 0xcc)
        {
            result = EatByteOrThrow();
            return true;
        }
        result = b;
        return false;
    }

    byte IDeserializer.ReadByte() => ReadByte();

    private byte ReadByte()
    {
        if (!TryReadByte(out var b))
        {
            throw new Exception($"Expected byte 0xcc, got 0x{b:x}");
        }
        return b;
    }

    char IDeserializer.ReadChar()
    {
        // char is encoded as either a 1-2 byte integer
        return (char)ReadU16();
    }

    IDeserializeCollection IDeserializer.ReadCollection(ISerdeInfo typeInfo)
    {
        var b = EatByteOrThrow();
        if (typeInfo.Kind == InfoKind.Enumerable)
        {
            int length;
            if (b <= 0x9f)
            {
                length = b & 0xf;
            }
            else if (b == 0xdc)
            {
                length = ReadBigEndianU16();
            }
            else if (b == 0xdd)
            {
                length = (int)ReadBigEndianU32();
            }
            else
            {
                throw new Exception($"Expected array, got 0x{b:x}");
            }
            return new DeserializeCollection(this, false, length);
        }
        else if (typeInfo.Kind == InfoKind.Dictionary)
        {
            int length;
            if (b <= 0x8f)
            {
                length = b & 0xf;
            }
            else if (b == 0xde)
            {
                length = ReadBigEndianU16();
            }
            else if (b == 0xdf)
            {
                length = (int)ReadBigEndianU32();
            }
            else
            {
                throw new Exception($"Expected dictionary, got 0x{b:x}");
            }
            return new DeserializeCollection(this, true, length*2);
        }
        else
        {
            throw new Exception("Expected collection");
        }
    }

    decimal IDeserializer.ReadDecimal()
    {
        throw new NotImplementedException();
    }

    private double ReadDouble()
    {
        var span = _reader.Span;
        if (span.Length < 9)
        {
            span = RefillNoEof(0);
        }
        var b = span[0];
        if (b != 0xcb)
        {
            throw new Exception($"Expected 64-bit double, got 0x{b:x}");
        }
        var result = BinaryPrimitives.ReadDoubleBigEndian(span[1..]);
        _reader.Advance(9);
        return result;
    }

    double IDeserializer.ReadDouble() => ReadDouble();

    private float ReadFloat()
    {
        var span = _reader.Span;
        if (span.Length < 5)
        {
            span = RefillNoEof(5);
        }
        var b = span[0];
        if (b != 0xca)
        {
            throw new Exception($"Expected 32-bit float, got 0x{b:x}");
        }
        var result = BinaryPrimitives.ReadSingleBigEndian(span[1..]);
        _reader.Advance(5);
        return result;
    }
    float IDeserializer.ReadFloat() => ReadFloat();

    private bool TryReadSbyte(out sbyte s)
    {
        var first = EatByteOrThrow();
        if (first <= 0x7f || first >= 0xe0)
        {
            s = (sbyte)first;
            return true;
        }
        if (first == 0xd0)
        {
            s = (sbyte)EatByteOrThrow();
            return true;
        }
        s = (sbyte)first;
        return false;
    }

    private bool TryReadI16(out short i16)
    {
        if (TryReadSbyte(out var sb))
        {
            i16 = sb;
            return true;
        }
        if (TryReadByte((byte)sb, out var b))
        {
            i16 = b;
            return true;
        }
        if (b == 0xd1)
        {
            i16 = (short)ReadBigEndianU16();
            return true;
        }
        i16 = b;
        return false;
    }

    short IDeserializer.ReadI16()
    {
        if (!TryReadI16(out var i16))
        {
            throw new Exception("Expected 16-bit integer");
        }
        return i16;
    }

    private bool TryReadI32(out int i32)
    {
        if (TryReadI16(out var i16))
        {
            i32 = i16;
            return true;
        }
        if (i16 == 0xcd)
        {
            i32 = ReadBigEndianU16();
            return true;
        }
        if (i16 == 0xd2)
        {
            i32 = (int)ReadBigEndianU32();
            return true;
        }
        i32 = i16;
        return false;
    }

    private int ReadI32()
    {
        if (!TryReadI32(out var i32))
        {
            throw new Exception("Expected 32-bit integer");
        }
        return i32;
    }

    int IDeserializer.ReadI32() => ReadI32();

    private bool TryReadI64(out long i64)
    {
        if (TryReadI32(out var i32))
        {
            i64 = i32;
            return true;
        }
        if (i32 == 0xce)
        {
            i64 = ReadBigEndianU32();
            return true;
        }
        if (i32 == 0xd3)
        {
            i64 = (long)ReadBigEndianU64();
            return true;
        }
        i64 = i32;
        return false;
    }

    private long ReadI64()
    {
        if (!TryReadI64(out var i64))
        {
            throw new Exception("Expected 64-bit integer");
        }
        return i64;
    }

    long IDeserializer.ReadI64() => ReadI64();

    T? IDeserializer.ReadNullableRef<T, TProxy>(TProxy proxy)
        where T : class
    {
        var b = PeekByteOrThrow();
        if (b == 0xc0)
        {
            _reader.Advance(1);
            return null;
        }
        return proxy.Deserialize(this);
    }

    sbyte IDeserializer.ReadSByte()
    {
        if (!TryReadSbyte(out var sb))
        {
            throw new Exception("Expected signed byte");
        }
        return sb;
    }

    string IDeserializer.ReadString() => ReadString();

    private string ReadString()
    {
        // strings are encoded in UTF8 as byte arrays
        var b = EatByteOrThrow();
        int length;
        if (b <= 0xbf)
        {
            length = b & 0x1f;
        }
        else if (b == 0xd9)
        {
            // 8-bit length
            length = EatByteOrThrow();
        }
        else if (b == 0xda)
        {
            // 16-bit length
            length = ReadBigEndianU16();
        }
        else if (b == 0xdb)
        {
            // 32-bit length
            length = (int)ReadBigEndianU32();
        }
        else
        {
            throw new Exception($"Expected string, got 0x{b:x}");
        }
        var span = _reader.Span;
        if (span.Length < length)
        {
            span = RefillNoEof(length);
        }
        var str = Encoding.UTF8.GetString(span[..length]);
        _reader.Advance(length);
        return str;
    }

    IDeserializeType IDeserializer.ReadType(ISerdeInfo typeInfo)
    {
        // Types are just an array of fields
        if (typeInfo.Kind == InfoKind.CustomType)
        {
            var fieldCount = typeInfo.FieldCount;
            var b = EatByteOrThrow();
            int length;
            if (fieldCount <= 0xff)
            {
                if (b > 0x9f)
                {
                    throw new Exception($"Expected array, got 0x{b:x}");
                }
                length = b & 0xf;
            }
            else if (fieldCount <= 0xffff)
            {
                if (b != 0xdc)
                {
                    throw new Exception($"Expected 16-bit array, got 0x{b:x}");
                }
                length = ReadBigEndianU16();
            }
            else
            {
                if (b != 0xdd)
                {
                    throw new Exception($"Expected 32-bit array, got 0x{b:x}");
                }
                length = (int)ReadBigEndianU32();
            }
            if (length != fieldCount)
            {
                throw new Exception($"Expected array of length {fieldCount}, got {length}");
            }
            return new DeserializeType(this);
        }
        else if (typeInfo.Kind == InfoKind.Enum)
        {
            return new DeserializeType(this);
        }
        throw new Exception("Expected custom type or enum");
    }

    private ushort ReadU16()
    {
        if (TryReadU16(out var u16))
        {
            return u16;
        }
        throw new Exception("Expected integer");
    }

    ushort IDeserializer.ReadU16() => ReadU16();

    private bool TryReadU16(out ushort u16)
    {
        if (TryReadByte(out var b))
        {
            u16 = b;
            return true;
        }
        if (b == 0xcd)
        {
            u16 = ReadBigEndianU16();
            return true;
        }
        u16 = b;
        return false;
    }

    private bool TryReadU32(out uint u32)
    {
        if (TryReadU16(out var u16))
        {
            u32 = u16;
            return true;
        }
        // u16 contains the first unexpected byte
        if (u16 == 0xce)
        {
            u32 = ReadBigEndianU32();
            return true;
        }
        u32 = u16;
        return false;
    }

    private bool TryReadU64(out ulong u64)
    {
        if (TryReadU32(out var u32))
        {
            u64 = u32;
            return true;
        }
        // u32 contains the first unexpected byte
        if (u32 == 0xcf)
        {
            u64 = ReadBigEndianU64();
            return true;
        }
        u64 = u32;
        return false;
    }

    uint IDeserializer.ReadU32()
    {
        if (!TryReadU32(out var u32))
        {
            throw new Exception($"Expected integer, got 0x{u32:x}");
        }
        return u32;
    }

    ulong IDeserializer.ReadU64()
    {
        if (!TryReadU64(out var u64))
        {
            throw new Exception($"Expected integer, got 0x{u64:x}");
        }
        return u64;
    }

    private ushort ReadBigEndianU16()
    {
        var span = _reader.Span;
        if (span.Length < 2)
        {
            span = RefillNoEof(2);
        }
        var result = BinaryPrimitives.ReadUInt16BigEndian(span);
        _reader.Advance(2);
        return result;
    }

    private uint ReadBigEndianU32()
    {
        var span = _reader.Span;
        if (span.Length < 4)
        {
            span = RefillNoEof(4);
        }
        var result = BinaryPrimitives.ReadUInt32BigEndian(span);
        _reader.Advance(4);
        return result;
    }

    private ulong ReadBigEndianU64()
    {
        var span = _reader.Span;
        if (span.Length < 8)
        {
            span = RefillNoEof(8);
        }
        var result = BinaryPrimitives.ReadUInt64BigEndian(span);
        _reader.Advance(8);
        return result;
    }
}