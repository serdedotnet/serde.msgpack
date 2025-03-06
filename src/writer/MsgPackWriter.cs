
using System.Buffers.Binary;
using System.ComponentModel;
using System.Text;

namespace Serde.MsgPack;

internal sealed partial class MsgPackWriter : ISerializer
{
    private readonly ScratchBuffer _out;
    private readonly EnumSerializer _enumSerializer;

    public MsgPackWriter(ScratchBuffer scratch)
    {
        _out = scratch;
        _enumSerializer = new EnumSerializer(this);
    }

    public void WriteBool(bool b)
    {
        _out.Add(b ? (byte)0xc3 : (byte)0xc2);
    }

    public void WriteByte(byte b) => WriteU64(b);

    public void WriteChar(char c) => WriteU64(c);

    public ISerializeCollection WriteCollection(ISerdeInfo typeInfo, int? length)
    {
        if (length is null)
        {
            throw new InvalidOperationException("Cannot serialize a collection with an unknown length.");
        }
        if (typeInfo.Kind == InfoKind.Enumerable)
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
        else if (typeInfo.Kind == InfoKind.Dictionary)
        {
            WriteMapLength(length);
        }
        else
        {
            throw new InvalidOperationException("Expected a collection, found: " + typeInfo.Kind);
        }
        return this;
    }

    private void WriteMapLength(int? length)
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
        throw new NotImplementedException();
    }

    public void WriteDouble(double d)
    {
        _out.Add(0xcb);
        WriteBigEndian(d);
    }

    public void WriteFloat(float f)
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

    public void WriteSByte(sbyte b) => WriteI64(b);

    public void WriteString(string s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        WriteUtf8String(bytes);
    }

    private void WriteUtf8String(ReadOnlySpan<byte> str)
    {
        if (str.Length <= 31)
        {
            _out.Add((byte)(0xa0 | str.Length));
        }
        else if (str.Length <= 0xff)
        {
            _out.Add(0xd9);
            _out.Add((byte)str.Length);
        }
        else if (str.Length <= 0xffff)
        {
            _out.Add(0xda);
            WriteBigEndian((ushort)str.Length);
        }
        else
        {
            _out.Add(0xdb);
            WriteBigEndian((uint)str.Length);
        }
        foreach (var b in str)
        {
            _out.Add(b);
        }
    }

    public ISerializeType WriteType(ISerdeInfo typeInfo)
    {
        if (typeInfo.Kind == InfoKind.CustomType)
        {
            // Custom types are serialized as a map
            WriteMapLength(typeInfo.FieldCount);
        }
        else if (typeInfo.Kind == InfoKind.Enum)
        {
            return _enumSerializer;
        }
        return this;
    }

    public void WriteU16(ushort u16) => WriteU64(u16);

    public void WriteU32(uint u32) => WriteU64(u32);

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
        _out.Add((byte)(value >> 8));
        _out.Add((byte)value);
    }

    private void WriteBigEndian(uint value)
    {
        _out.Add((byte)(value >> 24));
        _out.Add((byte)(value >> 16));
        _out.Add((byte)(value >> 8));
        _out.Add((byte)value);
    }

    private void WriteBigEndian(ulong value)
    {
        _out.Add((byte)(value >> 56));
        _out.Add((byte)(value >> 48));
        _out.Add((byte)(value >> 40));
        _out.Add((byte)(value >> 32));
        _out.Add((byte)(value >> 24));
        _out.Add((byte)(value >> 16));
        _out.Add((byte)(value >> 8));
        _out.Add((byte)value);
    }

    private void WriteBigEndian(short value) => WriteBigEndian((ushort)value);
    private void WriteBigEndian(int value) => WriteBigEndian((uint)value);
    private void WriteBigEndian(long value) => WriteBigEndian((ulong)value);
    private void WriteBigEndian(float f)
    {
        Span<byte> bytes = stackalloc byte[4];
        BinaryPrimitives.WriteSingleBigEndian(bytes, f);
        _out.AddRange(bytes);
    }
    private void WriteBigEndian(double d)
    {
        Span<byte> bytes = stackalloc byte[8];
        BinaryPrimitives.WriteDoubleBigEndian(bytes, d);
        _out.AddRange(bytes);
    }
}