using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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

    void IDisposable.Dispose() { }

    // Free lists for the per-type and per-collection deserializer helpers. These are
    // structurally tiny but were previously boxed once per node in the object graph
    // (a struct returned through ITypeDeserializer). Pooling them on the reader makes
    // sibling and nested nodes reuse instances, so total allocation drops from O(nodes)
    // to O(peak depth). Instances are rented in ReadType/ReadCollection and returned in
    // End(), which the generated proxies and the collection proxies always call exactly
    // once. The lists are intrusive (via _poolNext) to avoid any auxiliary allocation.
    private DeserializeType? _typeFreeList;
    private DeserializeCollection? _collFreeList;

    private DeserializeType RentType(bool compact, int length)
    {
        var t = _typeFreeList;
        if (t is null)
        {
            return new DeserializeType(this, compact, length);
        }
        _typeFreeList = t._poolNext;
        t.Reset(compact, length);
        return t;
    }

    private void ReturnType(DeserializeType t)
    {
        if (!t._inUse)
        {
            return;
        }
        t._inUse = false;
        t._poolNext = _typeFreeList;
        _typeFreeList = t;
    }

    private DeserializeCollection RentCollection(bool isDict, int length)
    {
        var c = _collFreeList;
        if (c is null)
        {
            return new DeserializeCollection(this, isDict, length);
        }
        _collFreeList = c._poolNext;
        c.Reset(isDict, length);
        return c;
    }

    private void ReturnCollection(DeserializeCollection c)
    {
        if (!c._inUse)
        {
            return;
        }
        c._inUse = false;
        c._poolNext = _collFreeList;
        _collFreeList = c;
    }

    [DoesNotReturn]
    private static void ThrowEof()
    {
        throw new Exception("Unexpected end of stream");
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
    /// Reads any MessagePack integer format and returns it as a signed 64-bit value.
    /// MessagePack stores integers in whichever int/uint format is smallest, so a
    /// reader for any width must accept every integer format.
    /// </summary>
    private long ReadInt64Token()
    {
        var b = EatByteOrThrow();
        if (b <= 0x7f)
        {
            return b; // positive fixint
        }
        if (b >= 0xe0)
        {
            return (sbyte)b; // negative fixint
        }
        switch (b)
        {
            case 0xcc:
                return EatByteOrThrow(); // uint 8
            case 0xcd:
                return ReadBigEndianU16(); // uint 16
            case 0xce:
                return ReadBigEndianU32(); // uint 32
            case 0xcf: // uint 64
                var u = ReadBigEndianU64();
                if (u > long.MaxValue)
                {
                    throw new Exception($"Integer {u} is too large for a signed 64-bit integer");
                }
                return (long)u;
            case 0xd0:
                return (sbyte)EatByteOrThrow(); // int 8
            case 0xd1:
                return (short)ReadBigEndianU16(); // int 16
            case 0xd2:
                return (int)ReadBigEndianU32(); // int 32
            case 0xd3:
                return (long)ReadBigEndianU64(); // int 64
            default:
                throw new Exception($"Expected integer, got 0x{b:x}");
        }
    }

    /// <summary>
    /// Reads any MessagePack integer format and returns it as an unsigned 64-bit value.
    /// Throws if the encoded value is negative.
    /// </summary>
    private ulong ReadUInt64Token()
    {
        var b = EatByteOrThrow();
        if (b <= 0x7f)
        {
            return b; // positive fixint
        }
        switch (b)
        {
            case 0xcc:
                return EatByteOrThrow(); // uint 8
            case 0xcd:
                return ReadBigEndianU16(); // uint 16
            case 0xce:
                return ReadBigEndianU32(); // uint 32
            case 0xcf:
                return ReadBigEndianU64(); // uint 64
            case 0xd0:
                return ToUnsigned((sbyte)EatByteOrThrow()); // int 8
            case 0xd1:
                return ToUnsigned((short)ReadBigEndianU16()); // int 16
            case 0xd2:
                return ToUnsigned((int)ReadBigEndianU32()); // int 32
            case 0xd3:
                return ToUnsigned((long)ReadBigEndianU64()); // int 64
            default:
                if (b >= 0xe0)
                {
                    throw new Exception($"Cannot read negative integer {(sbyte)b} as unsigned");
                }
                throw new Exception($"Expected integer, got 0x{b:x}");
        }

        static ulong ToUnsigned(long value)
        {
            if (value < 0)
            {
                throw new Exception($"Cannot read negative integer {value} as unsigned");
            }
            return (ulong)value;
        }
    }

    byte IDeserializer.ReadU8() => ReadU8();

    private byte ReadU8()
    {
        var v = ReadUInt64Token();
        if (v > byte.MaxValue)
        {
            throw new Exception($"Integer {v} is out of range for a byte");
        }
        return (byte)v;
    }

    public char ReadChar()
    {
        // char is encoded as either a 1-2 byte integer
        return (char)ReadU16();
    }

    private ITypeDeserializer ReadCollection(ISerdeInfo typeInfo)
    {
        var b = EatByteOrThrow();
        if (typeInfo.Kind == InfoKind.List)
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
            return RentCollection(false, length);
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
            return RentCollection(true, length * 2);
        }
        else
        {
            throw new Exception("Expected either List or Dictionary, found " + typeInfo.Kind);
        }
    }

    public decimal ReadDecimal()
    {
        return decimal.Parse(ReadString(), NumberStyles.Number, CultureInfo.InvariantCulture);
    }

    private double ReadF64()
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

    double IDeserializer.ReadF64() => ReadF64();

    private float ReadF32()
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

    float IDeserializer.ReadF32() => ReadF32();

    private short ReadI16()
    {
        var v = ReadInt64Token();
        if (v < short.MinValue || v > short.MaxValue)
        {
            throw new Exception($"Integer {v} is out of range for a 16-bit integer");
        }
        return (short)v;
    }

    short IDeserializer.ReadI16() => ReadI16();

    private int ReadI32()
    {
        var v = ReadInt64Token();
        if (v < int.MinValue || v > int.MaxValue)
        {
            throw new Exception($"Integer {v} is out of range for a 32-bit integer");
        }
        return (int)v;
    }

    int IDeserializer.ReadI32() => ReadI32();

    private long ReadI64() => ReadInt64Token();

    long IDeserializer.ReadI64() => ReadI64();

    T? IDeserializer.ReadNullableRef<T>(IDeserialize<T> proxy)
        where T : class
    {
        if (((IDeserializer)this).TryReadNull())
        {
            return null;
        }
        return proxy.Deserialize(this);
    }

    bool IDeserializer.TryReadNull()
    {
        var b = PeekByteOrThrow();
        if (b == 0xc0)
        {
            _reader.Advance(1);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Consumes a single nil (0xc0) token. Used to skip holes in the compact
    /// positional-array representation of custom types.
    /// </summary>
    private void SkipNil()
    {
        var b = EatByteOrThrow();
        if (b != 0xc0)
        {
            throw new Exception($"Expected nil (0xc0) for compact array hole, got 0x{b:x}");
        }
    }

    /// <summary>
    /// Skips one complete value of any type, including the contents of arrays and maps.
    /// </summary>
    private void SkipValue()
    {
        var b = EatByteOrThrow();
        switch (b)
        {
            case <= 0x7f: // positive fixint
            case >= 0xe0: // negative fixint
            case 0xc0 or 0xc2 or 0xc3: // nil, false, true
                return;
            case <= 0x8f: // fixmap
                SkipValues(2 * (b & 0x0f));
                return;
            case <= 0x9f: // fixarray
                SkipValues(b & 0x0f);
                return;
            case <= 0xbf: // fixstr
                SkipBytes(b & 0x1f);
                return;
            case 0xc4 or 0xd9: // bin 8, str 8
                SkipBytes(EatByteOrThrow());
                return;
            case 0xc5 or 0xda: // bin 16, str 16
                SkipBytes(ReadBigEndianU16());
                return;
            case 0xc6 or 0xdb: // bin 32, str 32
                SkipBytes((int)ReadBigEndianU32());
                return;
            case 0xc7: // ext 8: length, then the type and the data
                SkipBytes(EatByteOrThrow() + 1);
                return;
            case 0xc8: // ext 16
                SkipBytes(ReadBigEndianU16() + 1);
                return;
            case 0xc9: // ext 32
                SkipBytes((int)ReadBigEndianU32() + 1);
                return;
            case 0xcc or 0xd0: // uint 8, int 8
                SkipBytes(1);
                return;
            case 0xcd or 0xd1: // uint 16, int 16
                SkipBytes(2);
                return;
            case 0xca or 0xce or 0xd2: // float 32, uint 32, int 32
                SkipBytes(4);
                return;
            case 0xcb or 0xcf or 0xd3: // float 64, uint 64, int 64
                SkipBytes(8);
                return;
            case 0xd4: // fixext 1: the type and the data
                SkipBytes(2);
                return;
            case 0xd5: // fixext 2
                SkipBytes(3);
                return;
            case 0xd6: // fixext 4
                SkipBytes(5);
                return;
            case 0xd7: // fixext 8
                SkipBytes(9);
                return;
            case 0xd8: // fixext 16
                SkipBytes(17);
                return;
            case 0xdc: // array 16
                SkipValues(ReadBigEndianU16());
                return;
            case 0xdd: // array 32
                SkipValues((int)ReadBigEndianU32());
                return;
            case 0xde: // map 16
                SkipValues(2 * ReadBigEndianU16());
                return;
            case 0xdf: // map 32
                SkipValues(2 * (int)ReadBigEndianU32());
                return;
            default: // 0xc1 is never used
                throw new DeserializeException($"Invalid MessagePack format byte 0x{b:x}");
        }
    }

    private void SkipValues(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SkipValue();
        }
    }

    private void SkipBytes(int count)
    {
        if (_reader.Span.Length < count)
        {
            RefillNoEof(count);
        }
        _reader.Advance(count);
    }

    public sbyte ReadI8()
    {
        var v = ReadInt64Token();
        if (v < sbyte.MinValue || v > sbyte.MaxValue)
        {
            throw new Exception($"Integer {v} is out of range for a signed byte");
        }
        return (sbyte)v;
    }

    string IDeserializer.ReadString() => ReadString();

    private string ReadString()
    {
        // strings are encoded in UTF8 as byte arrays
        var span = ReadUtf8Span();
        var str = Encoding.UTF8.GetString(span);
        return str;
    }

    // Enums are encoded as a msgpack string of the variant name. Read the name and map it
    // back to the variant ordinal via the enum's SerdeInfo.
    int IDeserializer.ReadEnum(ISerdeInfo info)
    {
        var span = ReadUtf8Span();
        int index = info.TryGetIndex(span);
        if (index == ITypeDeserializer.IndexNotFound)
        {
            throw new Exception(
                $"Unknown enum member '{Encoding.UTF8.GetString(span)}' for enum '{info.Name}'"
            );
        }
        return index;
    }

    private const long BclSecondsAtUnixEpoch = 62135596800;

    public DateTime ReadDateTime()
    {
        long ticks = ReadTimestampTicks();
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Reads a msgpack timestamp extension (ext type -1) in any of the 32/64/96
    /// encodings and returns the corresponding BCL <see cref="DateTime.Ticks"/>.
    /// </summary>
    private long ReadTimestampTicks()
    {
        var b = EatByteOrThrow();
        uint nanoseconds;
        long seconds;
        switch (b)
        {
            case 0xd6: // fixext 4 -> timestamp 32
            {
                ExpectTimestampType();
                seconds = ReadBigEndianU32();
                nanoseconds = 0;
                break;
            }
            case 0xd7: // fixext 8 -> timestamp 64
            {
                ExpectTimestampType();
                ulong data64 = ReadBigEndianU64();
                nanoseconds = (uint)(data64 >> 34);
                seconds = (long)(data64 & 0x3ffffffffUL);
                break;
            }
            case 0xc7: // ext 8 -> timestamp 96
            {
                var len = EatByteOrThrow();
                if (len != 12)
                {
                    throw new Exception($"Expected timestamp 96 (length 12), got length {len}");
                }
                ExpectTimestampType();
                nanoseconds = ReadBigEndianU32();
                seconds = (long)ReadBigEndianU64();
                break;
            }
            default:
                throw new Exception($"Expected a timestamp extension, got 0x{b:x}");
        }

        return (seconds + BclSecondsAtUnixEpoch) * TimeSpan.TicksPerSecond + nanoseconds / 100;
    }

    private void ExpectTimestampType()
    {
        var type = EatByteOrThrow();
        if (type != 0xff)
        {
            throw new Exception($"Expected timestamp extension type -1 (0xff), got 0x{type:x}");
        }
    }

    public void ReadBytes(IBufferWriter<byte> writer)
    {
        var b = EatByteOrThrow();
        int length = b switch
        {
            0xc4 => EatByteOrThrow(),
            0xc5 => ReadBigEndianU16(), // 16-bit length
            0xc6 => checked((int)ReadBigEndianU32()), // 32-bit length
            _ => throw new DeserializeException($"Expected bytes, got 0x{b:x}"),
        };
        if (!_reader.FillBuffer(length))
        {
            ThrowEof();
        }
        var inputSpan = _reader.Span[..length];
        var outSpan = writer.GetSpan(length);
        inputSpan.CopyTo(outSpan);
        _reader.Advance(length);
        writer.Advance(length);
    }

    private ReadOnlySpan<byte> ReadUtf8Span()
    {
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
        _reader.Advance(length);
        return span[..length];
    }

    ITypeDeserializer IDeserializer.ReadType(ISerdeInfo typeInfo)
    {
        if (typeInfo.Kind == InfoKind.List || typeInfo.Kind == InfoKind.Dictionary)
        {
            return ReadCollection(typeInfo);
        }
        else if (typeInfo.Kind == InfoKind.CustomType)
        {
            if (typeInfo.HasExplicitFieldOrdinals)
            {
                // Compact representation: a positional array indexed by ordinal
                // (see CompactSerializer). Validate the array header; positions are
                // mapped back to fields by DeserializeType.ReadIndexWithName.
                var b = EatByteOrThrow();
                int length;
                if (b >= 0x90 && b <= 0x9f)
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
                var expected =
                    typeInfo.FieldCount == 0
                        ? 0
                        : typeInfo.GetFieldOrdinal(typeInfo.FieldCount - 1) + 1;
                if (length != expected)
                {
                    throw new Exception($"Expected array of length {expected}, got {length}");
                }
                return RentType(true, length);
            }

            // Custom types are serialized as maps (see WriteMapLength), with the
            // field names as keys. The keys/values are read by
            // DeserializeType.ReadIndexWithName, which reads as many entries as the map
            // has: more than the type's members means unknown members, and fewer means
            // omitted ones. Both are handled by the type's deserializer.
            var mb = EatByteOrThrow();
            int mlength;
            if (mb >= 0x80 && mb <= 0x8f)
            {
                mlength = mb & 0xf;
            }
            else if (mb == 0xde)
            {
                mlength = ReadBigEndianU16();
            }
            else if (mb == 0xdf)
            {
                mlength = (int)ReadBigEndianU32();
            }
            else
            {
                throw new Exception($"Expected map, got 0x{mb:x}");
            }
            return RentType(false, mlength);
        }
        throw new Exception("Expected custom type or enum");
    }

    private ushort ReadU16()
    {
        var v = ReadUInt64Token();
        if (v > ushort.MaxValue)
        {
            throw new Exception($"Integer {v} is out of range for a 16-bit unsigned integer");
        }
        return (ushort)v;
    }

    ushort IDeserializer.ReadU16() => ReadU16();

    public uint ReadU32()
    {
        var v = ReadUInt64Token();
        if (v > uint.MaxValue)
        {
            throw new Exception($"Integer {v} is out of range for a 32-bit unsigned integer");
        }
        return (uint)v;
    }

    public ulong ReadU64() => ReadUInt64Token();

    UInt128 IDeserializer.ReadU128() => ReadU128();

    private UInt128 ReadU128()
    {
        var span = ReadBinSpan();
        if (span.Length != 16)
        {
            throw new Exception($"Expected 16-byte integer, got {span.Length} bytes");
        }
        return BinaryPrimitives.ReadUInt128BigEndian(span);
    }

    Int128 IDeserializer.ReadI128() => ReadI128();

    private Int128 ReadI128()
    {
        var span = ReadBinSpan();
        if (span.Length != 16)
        {
            throw new Exception($"Expected 16-byte integer, got {span.Length} bytes");
        }
        return BinaryPrimitives.ReadInt128BigEndian(span);
    }

    DateTimeOffset IDeserializer.ReadDateTimeOffset() => ReadDateTimeOffset();

    private DateTimeOffset ReadDateTimeOffset()
    {
        // Matches MessagePack-CSharp: a 2-element array of the wall-clock time
        // (a timestamp ext) and the offset in minutes.
        var b = EatByteOrThrow();
        int length;
        if (b >= 0x90 && b <= 0x9f)
        {
            length = b & 0xf;
        }
        else if (b == 0xdc)
        {
            length = ReadBigEndianU16();
        }
        else
        {
            throw new Exception($"Expected a 2-element array for DateTimeOffset, got 0x{b:x}");
        }
        if (length != 2)
        {
            throw new Exception(
                $"Expected a 2-element array for DateTimeOffset, got length {length}"
            );
        }
        long ticks = ReadTimestampTicks();
        short offsetMinutes = (short)ReadInt64Token();
        return new DateTimeOffset(ticks, TimeSpan.FromMinutes(offsetMinutes));
    }

    private ReadOnlySpan<byte> ReadBinSpan()
    {
        var b = EatByteOrThrow();
        int length = b switch
        {
            0xc4 => EatByteOrThrow(),
            0xc5 => ReadBigEndianU16(),
            0xc6 => checked((int)ReadBigEndianU32()),
            _ => throw new Exception($"Expected bin, got 0x{b:x}"),
        };
        var span = _reader.Span;
        if (span.Length < length)
        {
            span = RefillNoEof(length);
        }
        _reader.Advance(length);
        return span[..length];
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
