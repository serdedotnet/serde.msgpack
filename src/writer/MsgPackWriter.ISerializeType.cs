
using Serde;

namespace Serde.MsgPack;

partial class MsgPackWriter : ISerializeType
{
    private void WritePropertyName(ISerdeInfo typeInfo, int fieldIndex)
    {
        var fieldName = typeInfo.GetFieldName(fieldIndex);
        WriteUtf8String(fieldName);
    }

    void ISerializeType.End(ISerdeInfo typeInfo)
    {
        // No end, all types are length-prefixed
    }

    void ISerializeType.WriteField<T>(ISerdeInfo typeInfo, int fieldIndex, T value, ISerialize<T> serialize)
    {
        WritePropertyName(typeInfo, fieldIndex);
        serialize.Serialize(value, this);
    }

    void ISerializeType.WriteBool(ISerdeInfo typeInfo, int index, bool b)
    {
        WritePropertyName(typeInfo, index);
        WriteBool(b);
    }

    void ISerializeType.WriteChar(ISerdeInfo typeInfo, int index, char c)
    {
        WritePropertyName(typeInfo, index);
        WriteChar(c);
    }

    void ISerializeType.WriteByte(ISerdeInfo typeInfo, int index, byte b)
    {
        WritePropertyName(typeInfo, index);
        WriteByte(b);
    }

    void ISerializeType.WriteU16(ISerdeInfo typeInfo, int index, ushort u16)
    {
        WritePropertyName(typeInfo, index);
        WriteU16(u16);
    }

    void ISerializeType.WriteU32(ISerdeInfo typeInfo, int index, uint u32)
    {
        WritePropertyName(typeInfo, index);
        WriteU32(u32);
    }

    void ISerializeType.WriteU64(ISerdeInfo typeInfo, int index, ulong u64)
    {
        WritePropertyName(typeInfo, index);
        WriteU64(u64);
    }

    void ISerializeType.WriteSByte(ISerdeInfo typeInfo, int index, sbyte b)
    {
        WritePropertyName(typeInfo, index);
        WriteSByte(b);
    }

    void ISerializeType.WriteI16(ISerdeInfo typeInfo, int index, short i16)
    {
        WritePropertyName(typeInfo, index);
        WriteI16(i16);
    }

    void ISerializeType.WriteI32(ISerdeInfo typeInfo, int index, int i32)
    {
        WritePropertyName(typeInfo, index);
        WriteI32(i32);
    }

    void ISerializeType.WriteI64(ISerdeInfo typeInfo, int index, long i64)
    {
        WritePropertyName(typeInfo, index);
        WriteI64(i64);
    }

    void ISerializeType.WriteFloat(ISerdeInfo typeInfo, int index, float f)
    {
        WritePropertyName(typeInfo, index);
        WriteFloat(f);
    }

    void ISerializeType.WriteDouble(ISerdeInfo typeInfo, int index, double d)
    {
        WritePropertyName(typeInfo, index);
        WriteDouble(d);
    }

    void ISerializeType.WriteDecimal(ISerdeInfo typeInfo, int index, decimal d)
    {
        WritePropertyName(typeInfo, index);
        WriteDecimal(d);
    }

    void ISerializeType.WriteString(ISerdeInfo typeInfo, int index, string s)
    {
        WritePropertyName(typeInfo, index);
        WriteString(s);
    }

    void ISerializeType.WriteNull(ISerdeInfo typeInfo, int index)
    {
        WritePropertyName(typeInfo, index);
        WriteNull();
    }
}