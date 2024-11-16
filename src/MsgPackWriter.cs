
using System.ComponentModel;
using System.Text;

namespace Serde.MsgPack;

internal sealed partial class MsgPackWriter(ScratchBuffer outBuffer) : ISerializer
{
    private readonly ScratchBuffer _out = outBuffer;

    void ISerializer.SerializeBool(bool b)
    {
        _out.Add(b ? (byte)0xc3 : (byte)0xc2);
    }

    void ISerializer.SerializeByte(byte b) => SerializeU64(b);

    void ISerializer.SerializeChar(char c) => SerializeU64(c);

    ISerializeCollection ISerializer.SerializeCollection(ISerdeInfo typeInfo, int? length)
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
        else
        {
            throw new InvalidOperationException("Expected a collection, found: " + typeInfo.Kind);
        }
        return this;
    }

    void ISerializer.SerializeDecimal(decimal d)
    {
        throw new NotImplementedException();
    }

    void ISerializer.SerializeDouble(double d)
    {
        _out.Add(0xcb);
        WriteBigEndian(d);
    }

    void ISerializer.SerializeEnumValue<T, U>(ISerdeInfo typeInfo, int index, T value, U serialize)
    {
        // Serialize the underlying value
        serialize.Serialize(value, this);
    }

    void ISerializer.SerializeFloat(float f)
    {
        _out.Add(0xca);
        WriteBigEndian(f);
    }

    void ISerializer.SerializeI16(short i16) => SerializeI64(i16);

    void ISerializer.SerializeI32(int i32) => SerializeI64(i32);

    void ISerializer.SerializeI64(long i64) => SerializeI64(i64);

    private void SerializeI64(long i64)
    {
        if (i64 >= 0)
        {
            SerializeU64((ulong)i64);
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

    void ISerializer.SerializeNull()
    {
        _out.Add(0xc0);
    }

    void ISerializer.SerializeSByte(sbyte b) => SerializeI64(b);

    void ISerializer.SerializeString(string s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        if (bytes.Length <= 31)
        {
            _out.Add((byte)(0xa0 | bytes.Length));
        }
        else if (bytes.Length <= 0xff)
        {
            _out.Add(0xd9);
            _out.Add((byte)bytes.Length);
        }
        else if (bytes.Length <= 0xffff)
        {
            _out.Add(0xda);
            WriteBigEndian((ushort)bytes.Length);
        }
        else
        {
            _out.Add(0xdb);
            WriteBigEndian((uint)bytes.Length);
        }
        foreach (var b in bytes)
        {
            _out.Add(b);
        }
    }

    ISerializeType ISerializer.SerializeType(ISerdeInfo typeInfo)
    {
        // Check that, if the members are marked with [Key], they are in order.
        // We do not support out-of-order keys.
        for (int i = 0; i < typeInfo.FieldCount; i++)
        {
            var attrs = typeInfo.GetFieldAttributes(i);
            foreach (var attr in attrs)
            {
                if (attr.AttributeType.FullName == "MessagePack.KeyAttribute")
                {
                    if (attr.ConstructorArguments is [ { Value: int index } ] && index != i)
                    {
                        throw new InvalidOperationException($"Found member {typeInfo.GetFieldStringName(i)} declared at index {i} but marked with [Key({index})]. Key indices must match declaration order.");
                    }
                }
            }
        }

        // Write as an array, with the keys left implicit in the order
        if (typeInfo.FieldCount <= 15)
        {
            _out.Add((byte)(0x90 | typeInfo.FieldCount));
        }
        else if (typeInfo.FieldCount <= 0xffff)
        {
            _out.Add(0xdc);
            WriteBigEndian((ushort)typeInfo.FieldCount);
        }
        else
        {
            _out.Add(0xdd);
            WriteBigEndian((uint)typeInfo.FieldCount);
        }
        return this;
    }

    void ISerializer.SerializeU16(ushort u16) => SerializeU64(u16);

    void ISerializer.SerializeU32(uint u32) => SerializeU64(u32);

    private void SerializeU64(ulong u64)
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

    void ISerializer.SerializeU64(ulong u64) => SerializeU64(u64);

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
    private void WriteBigEndian(double d)
    {
        var bytes = BitConverter.GetBytes(d);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        _out.AddRange(bytes);
    }
}