
namespace Serde.MsgPack;

partial class MsgPackReader<TReader>
{
    private struct DeserializeType(MsgPackReader<TReader> deserializer) : IDeserializeType
    {
        private int _count;

        bool IDeserializeType.ReadBool(int index) => deserializer.ReadBool();

        byte IDeserializeType.ReadByte(int index) => deserializer.ReadByte();

        char IDeserializeType.ReadChar(int index) => (char)deserializer.ReadU16();

        decimal IDeserializeType.ReadDecimal(int index) => deserializer.ReadDecimal();
        double IDeserializeType.ReadDouble(int index) => deserializer.ReadDouble();

        float IDeserializeType.ReadFloat(int index) => deserializer.ReadFloat();

        short IDeserializeType.ReadI16(int index) => deserializer.ReadI16();

        int IDeserializeType.ReadI32(int index) => deserializer.ReadI32();

        long IDeserializeType.ReadI64(int index) => deserializer.ReadI64();

        sbyte IDeserializeType.ReadSByte(int index) => deserializer.ReadSByte();

        string IDeserializeType.ReadString(int index) => deserializer.ReadString();

        ushort IDeserializeType.ReadU16(int index) => deserializer.ReadU16();

        uint IDeserializeType.ReadU32(int index) => deserializer.ReadU32();

        ulong IDeserializeType.ReadU64(int index) => deserializer.ReadU64();

        T IDeserializeType.ReadValue<T, D>(int index, D deserialize)
             => deserialize.Deserialize(deserializer);

        void IDeserializeType.SkipValue()
            => throw new NotImplementedException();

        int IDeserializeType.TryReadIndex(ISerdeInfo map, out string? errorName)
        {
            // Two options: we have a struct/class, or an enum
            if (map.Kind == InfoKind.CustomType)
            {
                if (_count >= map.FieldCount)
                {
                    errorName = null;
                    return IDeserializeType.EndOfType;
                }
                // custom types are serialized like maps with field names as keys
                var span = deserializer.ReadUtf8Span();
                int index = map.TryGetIndex(span);
                errorName = index == IDeserializeType.IndexNotFound ? span.ToString() : null;
                _count++;
                return index;
            }
            else if (map.Kind == InfoKind.Enum)
            {
                // Enums are serialized as the index of the enum member
                errorName = null;
                return deserializer.ReadI32();
            }
            else
            {
                errorName = "Expected a custom type or enum, found: " + map.Kind;
                return IDeserializeType.IndexNotFound;
            }
        }
    }
}