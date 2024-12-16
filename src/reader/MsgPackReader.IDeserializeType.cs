
namespace Serde.MsgPack;

partial class MsgPackReader<TReader>
{
    private struct DeserializeType(MsgPackReader<TReader> deserializer) : IDeserializeType
    {
        private int _index;

        bool IDeserializeType.ReadBool(int index) => deserializer.ReadBool();

        byte IDeserializeType.ReadByte(int index) => deserializer.ReadByte();

        char IDeserializeType.ReadChar(int index) => (char)deserializer.ReadU16();

        decimal IDeserializeType.ReadDecimal(int index)
        {
            throw new NotImplementedException();
        }

        double IDeserializeType.ReadDouble(int index)
        {
            throw new NotImplementedException();
        }

        float IDeserializeType.ReadFloat(int index)
        {
            throw new NotImplementedException();
        }

        short IDeserializeType.ReadI16(int index)
        {
            throw new NotImplementedException();
        }

        int IDeserializeType.ReadI32(int index) => deserializer.ReadI32();

        long IDeserializeType.ReadI64(int index) => deserializer.ReadI64();

        sbyte IDeserializeType.ReadSByte(int index)
        {
            throw new NotImplementedException();
        }

        string IDeserializeType.ReadString(int index) => deserializer.ReadString();

        ushort IDeserializeType.ReadU16(int index)
        {
            throw new NotImplementedException();
        }

        uint IDeserializeType.ReadU32(int index)
        {
            throw new NotImplementedException();
        }

        ulong IDeserializeType.ReadU64(int index)
        {
            throw new NotImplementedException();
        }

        T IDeserializeType.ReadValue<T>(int index, Serde.IDeserialize<T> deserialize)
        {
            throw new NotImplementedException();
        }

        void IDeserializeType.SkipValue()
        {
            throw new NotImplementedException();
        }

        int IDeserializeType.TryReadIndex(ISerdeInfo map, out string? errorName)
        {
            // Two options: we have a struct/class, or an enum
            if (map.Kind == InfoKind.CustomType)
            {
                // custom types always serialize in order, so just increment the index counter
                if (_index >= map.FieldCount)
                {
                    errorName = null;
                    return IDeserializeType.EndOfType;
                }
                errorName = null;
                return _index++;
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