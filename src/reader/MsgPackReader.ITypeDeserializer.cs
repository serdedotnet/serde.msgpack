
using System.Buffers;

namespace Serde.MsgPack;

partial class MsgPackReader<TReader>
{
    private struct DeserializeType(MsgPackReader<TReader> deserializer) : ITypeDeserializer
    {
        int? ITypeDeserializer.SizeOpt => null;

        private int _count;

        bool ITypeDeserializer.ReadBool(ISerdeInfo info, int index) => deserializer.ReadBool();

        byte ITypeDeserializer.ReadU8(ISerdeInfo info, int index) => deserializer.ReadU8();

        char ITypeDeserializer.ReadChar(ISerdeInfo info, int index) => (char)deserializer.ReadU16();

        decimal ITypeDeserializer.ReadDecimal(ISerdeInfo info, int index) => deserializer.ReadDecimal();
        double ITypeDeserializer.ReadF64(ISerdeInfo info, int index) => deserializer.ReadF64();

        float ITypeDeserializer.ReadF32(ISerdeInfo info, int index) => deserializer.ReadF32();

        short ITypeDeserializer.ReadI16(ISerdeInfo info, int index) => deserializer.ReadI16();

        int ITypeDeserializer.ReadI32(ISerdeInfo info, int index) => deserializer.ReadI32();

        long ITypeDeserializer.ReadI64(ISerdeInfo info, int index) => deserializer.ReadI64();

        sbyte ITypeDeserializer.ReadI8(ISerdeInfo info, int index) => deserializer.ReadI8();

        string ITypeDeserializer.ReadString(ISerdeInfo info, int index) => deserializer.ReadString();

        ushort ITypeDeserializer.ReadU16(ISerdeInfo info, int index) => deserializer.ReadU16();

        uint ITypeDeserializer.ReadU32(ISerdeInfo info, int index) => deserializer.ReadU32();

        ulong ITypeDeserializer.ReadU64(ISerdeInfo info, int index) => deserializer.ReadU64();

        UInt128 ITypeDeserializer.ReadU128(ISerdeInfo info, int index) => deserializer.ReadU128();

        Int128 ITypeDeserializer.ReadI128(ISerdeInfo info, int index) => deserializer.ReadI128();

        DateTimeOffset ITypeDeserializer.ReadDateTimeOffset(ISerdeInfo info, int index) => deserializer.ReadDateTimeOffset();

        T ITypeDeserializer.ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> deserialize)
             => deserialize.Deserialize(deserializer);

        void ITypeDeserializer.SkipValue(ISerdeInfo info, int index)
            => throw new NotImplementedException();

        int ITypeDeserializer.TryReadIndex(ISerdeInfo map)
            => ReadIndexWithName(map).Item1;

        (int, string?) ITypeDeserializer.TryReadIndexWithName(ISerdeInfo map)
            => ReadIndexWithName(map);

        private (int, string?) ReadIndexWithName(ISerdeInfo map)
        {
            // Two options: we have a struct/class, or an enum
            if (map.Kind == InfoKind.CustomType)
            {
                if (_count >= map.FieldCount)
                {
                    return (ITypeDeserializer.EndOfType, null);
                }
                // custom types are serialized like maps with field names as keys
                var span = deserializer.ReadUtf8Span();
                int index = map.TryGetIndex(span);
                var errorName = index == ITypeDeserializer.IndexNotFound ? span.ToString() : null;
                _count++;
                return (index, errorName);
            }
            else if (map.Kind == InfoKind.Enum)
            {
                // Enums are serialized as the index of the enum member
                return (deserializer.ReadI32(), null);
            }
            else
            {
                return (ITypeDeserializer.IndexNotFound, "Expected a custom type or enum, found: " + map.Kind);
            }
        }

        DateTime ITypeDeserializer.ReadDateTime(ISerdeInfo info, int index)
            => deserializer.ReadDateTime();

        void ITypeDeserializer.ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer)
        {
            deserializer.ReadBytes(writer);
        }
    }
}