using System.Buffers;

namespace Serde.MsgPack;

partial class MsgPackReader<TReader>
{
    private sealed class DeserializeType : ITypeDeserializer
    {
        private readonly MsgPackReader<TReader> deserializer;

        // Intrusive free-list link and in-use guard for pooling (see RentType/ReturnType).
        internal DeserializeType? _poolNext;
        internal bool _inUse;

        int? ITypeDeserializer.SizeOpt => null;

        private int _count;

        // Compact (positional-array) mode state. When _compact is true, the type
        // was written as an array indexed by field ordinal (see CompactSerializer);
        // _pos is the current array position and _length the array length.
        private bool _compact;
        private int _length;
        private int _pos;

        public DeserializeType(MsgPackReader<TReader> deserializer, bool compact, int length)
        {
            this.deserializer = deserializer;
            Reset(compact, length);
        }

        public void Reset(bool compact, int length)
        {
            _count = 0;
            _compact = compact;
            _length = length;
            _pos = 0;
            _inUse = true;
        }

        void ITypeDeserializer.End(ISerdeInfo info) => deserializer.ReturnType(this);

        bool ITypeDeserializer.ReadBool(ISerdeInfo info, int index) => deserializer.ReadBool();

        byte ITypeDeserializer.ReadU8(ISerdeInfo info, int index) => deserializer.ReadU8();

        char ITypeDeserializer.ReadChar(ISerdeInfo info, int index) => (char)deserializer.ReadU16();

        decimal ITypeDeserializer.ReadDecimal(ISerdeInfo info, int index) =>
            deserializer.ReadDecimal();

        double ITypeDeserializer.ReadF64(ISerdeInfo info, int index) => deserializer.ReadF64();

        float ITypeDeserializer.ReadF32(ISerdeInfo info, int index) => deserializer.ReadF32();

        short ITypeDeserializer.ReadI16(ISerdeInfo info, int index) => deserializer.ReadI16();

        int ITypeDeserializer.ReadI32(ISerdeInfo info, int index) => deserializer.ReadI32();

        long ITypeDeserializer.ReadI64(ISerdeInfo info, int index) => deserializer.ReadI64();

        sbyte ITypeDeserializer.ReadI8(ISerdeInfo info, int index) => deserializer.ReadI8();

        string ITypeDeserializer.ReadString(ISerdeInfo info, int index) =>
            deserializer.ReadString();

        ushort ITypeDeserializer.ReadU16(ISerdeInfo info, int index) => deserializer.ReadU16();

        uint ITypeDeserializer.ReadU32(ISerdeInfo info, int index) => deserializer.ReadU32();

        ulong ITypeDeserializer.ReadU64(ISerdeInfo info, int index) => deserializer.ReadU64();

        UInt128 ITypeDeserializer.ReadU128(ISerdeInfo info, int index) => deserializer.ReadU128();

        Int128 ITypeDeserializer.ReadI128(ISerdeInfo info, int index) => deserializer.ReadI128();

        DateTimeOffset ITypeDeserializer.ReadDateTimeOffset(ISerdeInfo info, int index) =>
            deserializer.ReadDateTimeOffset();

        T ITypeDeserializer.ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> deserialize) =>
            deserialize.Deserialize(deserializer);

        int ITypeDeserializer.ReadEnum(ISerdeInfo typeInfo, int index, ISerdeInfo fieldInfo) =>
            ((IDeserializer)deserializer).ReadEnum(fieldInfo);

        IDeserializer ITypeDeserializer.ReadFieldStart(ISerdeInfo info, int index) => deserializer;

        void ITypeDeserializer.ReadFieldEnd(
            ISerdeInfo info,
            int index,
            IDeserializer deserializer
        ) { }

        void ITypeDeserializer.SkipValue(ISerdeInfo info, int index) =>
            throw new NotImplementedException();

        int ITypeDeserializer.TryReadIndex(ISerdeInfo map) => ReadIndexWithName(map).Item1;

        (int, string?) ITypeDeserializer.TryReadIndexWithName(ISerdeInfo map) =>
            ReadIndexWithName(map);

        private (int, string?) ReadIndexWithName(ISerdeInfo map)
        {
            // Two options: we have a struct/class, or an enum
            if (map.Kind == InfoKind.CustomType)
            {
                if (_compact)
                {
                    // Compact mode: walk array positions, mapping each to its field
                    // by ordinal and consuming nil placeholders for holes.
                    while (_pos < _length)
                    {
                        if (_count < map.FieldCount && map.GetFieldOrdinal(_count) == _pos)
                        {
                            _pos++;
                            return (_count++, null);
                        }
                        deserializer.SkipNil();
                        _pos++;
                    }
                    return (ITypeDeserializer.EndOfType, null);
                }
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
            else
            {
                return (
                    ITypeDeserializer.IndexNotFound,
                    "Expected a custom type, found: " + map.Kind
                );
            }
        }

        DateTime ITypeDeserializer.ReadDateTime(ISerdeInfo info, int index) =>
            deserializer.ReadDateTime();

        void ITypeDeserializer.ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer)
        {
            deserializer.ReadBytes(writer);
        }
    }
}
