
using System.Buffers.Binary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Serde.MsgPack;

internal sealed partial class MsgPackWriter : ISerializer
{
    private readonly ScratchBuffer _out;
    private CompactSerializer? _compactSerializer;

    public MsgPackWriter(ScratchBuffer scratch)
    {
        _out = scratch;
    }

    public void WriteBool(bool b)
    {
        _out.Add(b ? (byte)0xc3 : (byte)0xc2);
    }

    public void WriteU8(byte b) => WriteU64(b);

    public void WriteChar(char c) => WriteU64(c);

    ITypeSerializer ISerializer.WriteCollection(ISerdeInfo typeInfo, int? length)
    {
        if (length is null)
        {
            throw new InvalidOperationException("Cannot serialize a collection with an unknown length.");
        }
        if (typeInfo.Kind == InfoKind.List)
        {
            WriteArrayLength((int)length);
        }
        else if (typeInfo.Kind == InfoKind.Dictionary)
        {
            WriteMapLength((int)length);
        }
        else
        {
            throw new InvalidOperationException("Expected a collection, found: " + typeInfo.Kind);
        }
        return new SerCollection(this);
    }

    private void WriteArrayLength(int length)
    {
        if (length <= 15)
        {
            _out.Add((byte)(0x90 | length));
        }
        else if (length <= 0xffff)
        {
            _out.Add(0xdc);
            WriteBigEndian((ushort)length);
        }
        else
        {
            _out.Add(0xdd);
            WriteBigEndian((uint)length);
        }
    }

    private void WriteMapLength(int length)
    {
        if (length <= 15)
        {
            _out.Add((byte)(0x80 | length));
        }
        else if (length <= 0xffff)
        {
            _out.Add(0xde);
            WriteBigEndian((ushort)length);
        }
        else
        {
            _out.Add(0xdf);
            WriteBigEndian((uint)length);
        }
    }

    public void WriteDecimal(decimal d)
    {
        // Match MessagePack-CSharp, which serializes decimal as its invariant string form.
        WriteString(d.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    public void WriteF64(double d)
    {
        _out.Add(0xcb);
        WriteBigEndian(d);
    }

    public void WriteF32(float f)
    {
        _out.Add(0xca);
        WriteBigEndian(f);
    }

    public void WriteI16(short i16) => WriteI64(i16);

    public void WriteI32(int i32) => WriteI64(i32);

    void ISerializer.WriteI64(long i64) => WriteI64(i64);

    private void WriteI64(long i64)
    {
        if (i64 >= 0)
        {
            WriteU64((ulong)i64);
        }
        else if (i64 >= -32)
        {
            _out.Add((byte)(0xe0 | (i64 + 32)));
        }
        else if (i64 >= -128)
        {
            _out.Add(0xd0);
            _out.Add((byte)i64);
        }
        else if (i64 >= -32768)
        {
            _out.Add(0xd1);
            WriteBigEndian((short)i64);
        }
        else if (i64 >= -2147483648)
        {
            _out.Add(0xd2);
            WriteBigEndian((int)i64);
        }
        else
        {
            _out.Add(0xd3);
            WriteBigEndian(i64);
        }
    }
    public void WriteNull()
    {
        _out.Add(0xc0);
    }

    // Number of seconds between 0001-01-01 (BCL DateTime epoch) and the Unix
    // epoch (1970-01-01). Used to translate DateTime.Ticks into the msgpack
    // timestamp extension's Unix-relative seconds.
    private const long BclSecondsAtUnixEpoch = 62135596800;
    private const long NanosecondsPerTick = 100;

    public void WriteDateTimeOffset(DateTimeOffset dt)
    {
        // Matches MessagePack-CSharp: a 2-element array of the wall-clock time
        // (encoded as a timestamp ext, treated as UTC) and the offset in minutes.
        WriteArrayLength(2);
        WriteTimestamp(dt.Ticks);
        WriteI16(checked((short)(dt.Offset.Ticks / TimeSpan.TicksPerMinute)));
    }

    public void WriteDateTime(DateTime dt)
    {
        // The msgpack timestamp extension encodes an absolute instant relative to
        // the Unix epoch in UTC (msgpack spec); it cannot represent a zone or a
        // DateTimeKind. A Local value would serialize to machine-timezone-dependent
        // bytes and an Unspecified value has no defined instant at all, so we reject
        // anything that isn't already UTC rather than silently converting.
        if (dt.Kind != DateTimeKind.Utc)
        {
            throw new InvalidOperationException(
                $"Cannot serialize a DateTime with Kind={dt.Kind}; the msgpack timestamp "
                + "extension only represents UTC instants. Convert the value to UTC "
                + "(e.g. DateTime.ToUniversalTime() or DateTime.SpecifyKind(value, DateTimeKind.Utc)) first.");
        }
        WriteTimestamp(dt.Ticks);
    }

    /// <summary>
    /// Writes BCL <paramref name="ticks"/> as a msgpack timestamp extension
    /// (ext type -1), choosing the timestamp 32/64/96 encoding exactly as the
    /// msgpack spec and MessagePack-CSharp do.
    /// </summary>
    private void WriteTimestamp(long ticks)
    {
        long secondsSinceBclEpoch = ticks / TimeSpan.TicksPerSecond;
        long seconds = secondsSinceBclEpoch - BclSecondsAtUnixEpoch;
        long nanoseconds = (ticks % TimeSpan.TicksPerSecond) * NanosecondsPerTick;

        if ((seconds >> 34) == 0)
        {
            ulong data64 = ((ulong)nanoseconds << 34) | (ulong)seconds;
            if ((data64 & 0xffffffff00000000UL) == 0)
            {
                // timestamp 32
                _out.Add(0xd6);
                _out.Add(0xff);
                WriteBigEndian((uint)data64);
            }
            else
            {
                // timestamp 64
                _out.Add(0xd7);
                _out.Add(0xff);
                WriteBigEndian(data64);
            }
        }
        else
        {
            // timestamp 96
            _out.Add(0xc7);
            _out.Add(0x0c);
            _out.Add(0xff);
            WriteBigEndian((uint)nanoseconds);
            WriteBigEndian(seconds);
        }
    }

    public void WriteBytes(ReadOnlyMemory<byte> bytes)
    {
        byte code = bytes.Length switch
        {
            <= 0xff => 0xc4,
            <= 0xffff => 0xc5,
            _ => 0xc6
        };
        var prefixLen = code switch
        {
            0xc4 => 2,
            0xc5 => 3,
            _ => 5
        };
        var span = _out.GetAppendSpan(prefixLen + bytes.Length);
        _out.Count += prefixLen + bytes.Length;
        span[0] = code;
        switch (prefixLen)
        {
            case 2:
                span[1] = (byte)bytes.Length;
                break;
            case 3:
                BinaryPrimitives.WriteUInt16BigEndian(span.Slice(1), (ushort)bytes.Length);
                break;
            case 5:
                BinaryPrimitives.WriteUInt32BigEndian(span.Slice(1), (uint)bytes.Length);
                break;
        }
        bytes.Span.CopyTo(span[prefixLen..]);
    }

    public void WriteI8(sbyte b) => WriteI64(b);

    private static readonly Encoding _utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    public void WriteString(string s)
    {
        // Single-pass encode: reserve worst-case space, then optimistically
        // size the length prefix assuming an ASCII (1 byte/char) encoding -- the
        // common case -- and encode the body once at that offset. Only if multi
        // byte characters push the UTF-8 byte count into a larger prefix class do
        // we shift the body (Span.CopyTo uses memmove and handles overlap).
        int charLen = s.Length;
        int maxByteCount = _utf8.GetMaxByteCount(charLen);
        var appendSpan = _out.GetAppendSpan(checked(maxByteCount + 5 /* max length prefix */));
        int guessOffset = Utf8HeaderSize(charLen);
        int byteCount = _utf8.GetBytes(s, appendSpan.Slice(guessOffset, maxByteCount));
        int actualOffset = Utf8HeaderSize(byteCount);
        if (actualOffset != guessOffset)
        {
            appendSpan.Slice(guessOffset, byteCount).CopyTo(appendSpan.Slice(actualOffset, byteCount));
        }
        WriteUtf8Header(byteCount, appendSpan);
        _out.Count += actualOffset + byteCount;
    }

    private void WriteUtf8(ReadOnlySpan<byte> str)
    {
        var span = _out.GetAppendSpan(str.Length + 5);
        int offset = WriteUtf8Header(str.Length, span);
        str.CopyTo(span.Slice(offset, str.Length));
        _out.Count += offset + str.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Utf8HeaderSize(int length) => length switch
    {
        <= 31 => 1,
        <= 0xff => 2,
        <= 0xffff => 3,
        _ => 5
    };

    /// <summary>
    /// Assumes that span is large enough to hold the header.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int WriteUtf8Header(int length, Span<byte> span)
    {
        int offset;
        if (length <= 31)
        {
            offset = 1;
            span[0] = (byte)(0xa0 | length);
        }
        else if (length <= 0xff)
        {
            offset = 2;
            span[0] = 0xd9;
            span[1] = unchecked((byte)length);
        }
        else if (length <= 0xffff)
        {
            offset = 3;
            span[0] = 0xda;
            BinaryPrimitives.WriteUInt16BigEndian(span.Slice(1), (ushort)length);
        }
        else
        {
            offset = 5;
            span[0] = 0xdb;
            BinaryPrimitives.WriteUInt32BigEndian(span.Slice(1), (uint)length);
        }
        return offset;
    }

    ITypeSerializer ISerializer.WriteType(ISerdeInfo typeInfo)
    {
        switch (typeInfo.Kind)
        {
            case InfoKind.CustomType:
                if (typeInfo.HasExplicitFieldOrdinals)
                {
                    // Compact representation: serialize as a positional array indexed by
                    // ordinal (matching MessagePack-CSharp's integer-key encoding). The
                    // array length is the largest ordinal + 1; holes are filled with nil.
                    var fieldCount = typeInfo.FieldCount;
                    int length = fieldCount == 0 ? 0 : typeInfo.GetFieldOrdinal(fieldCount - 1) + 1;
                    WriteArrayLength(length);
                    var ser = _compactSerializer ??= new CompactSerializer(this);
                    ser.Begin(length);
                    return ser;
                }
                // Otherwise serialize as a map keyed by field name.
                WriteMapLength(typeInfo.FieldCount);
                return this;
        }
        throw new InvalidOperationException("Unexpected info kind: " + typeInfo.Kind);
    }

    // Enums are serialized as a msgpack string of the variant name (the field name at
    // the given ordinal in the enum's SerdeInfo), matching serde's name-based enum model.
    public void WriteEnum(ISerdeInfo info, int ordinal)
    {
        WriteUtf8(info.GetFieldName(ordinal));
    }

    public void WriteU16(ushort u16) => WriteU64(u16);

    public void WriteU32(uint u32) => WriteU64(u32);

    public void WriteU128(UInt128 u128)
    {
        Span<byte> bytes = stackalloc byte[16];
        BinaryPrimitives.WriteUInt128BigEndian(bytes, u128);
        WriteRawBin(bytes);
    }

    public void WriteI128(Int128 i128)
    {
        Span<byte> bytes = stackalloc byte[16];
        BinaryPrimitives.WriteInt128BigEndian(bytes, i128);
        WriteRawBin(bytes);
    }

    private void WriteRawBin(ReadOnlySpan<byte> bytes)
    {
        var span = _out.GetAppendSpan(2 + bytes.Length);
        span[0] = 0xc4;
        span[1] = (byte)bytes.Length;
        bytes.CopyTo(span[2..]);
        _out.Count += 2 + bytes.Length;
    }

    void ISerializer.WriteU64(ulong u64) => WriteU64(u64);

    private void WriteU64(ulong u64)
    {
        if (u64 <= 0x7f)
        {
            _out.Add((byte)u64);
        }
        else if (u64 <= 0xff)
        {
            _out.Add(0xcc);
            _out.Add((byte)u64);
        }
        else if (u64 <= 0xffff)
        {
            _out.Add(0xcd);
            WriteBigEndian((ushort)u64);
        }
        else if (u64 <= 0xffffffff)
        {
            _out.Add(0xce);
            WriteBigEndian((uint)u64);
        }
        else
        {
            _out.Add(0xcf);
            WriteBigEndian(u64);
        }
    }

    private void WriteBigEndian(ushort value)
    {
        var span = _out.GetAppendSpan(2);
        BinaryPrimitives.WriteUInt16BigEndian(
            span,
            value);
        _out.Count += 2;
    }

    private void WriteBigEndian(uint value)
    {
        var span = _out.GetAppendSpan(4);
        BinaryPrimitives.WriteUInt32BigEndian(
            span,
            value
        );
        _out.Count += 4;
    }

    private void WriteBigEndian(ulong value)
    {
        var span = _out.GetAppendSpan(8);
        BinaryPrimitives.WriteUInt64BigEndian(
            span,
            value
        );
        _out.Count += 8;
    }

    private void WriteBigEndian(short value) => WriteBigEndian((ushort)value);
    private void WriteBigEndian(int value) => WriteBigEndian((uint)value);
    private void WriteBigEndian(long value) => WriteBigEndian((ulong)value);
    private void WriteBigEndian(float value)
    {
        var span = _out.GetAppendSpan(4);
        BinaryPrimitives.WriteSingleBigEndian(span, value);
        _out.Count += 4;
    }
    private void WriteBigEndian(double value)
    {
        var span = _out.GetAppendSpan(8);
        BinaryPrimitives.WriteDoubleBigEndian(span, value);
        _out.Count += 8;
    }
}