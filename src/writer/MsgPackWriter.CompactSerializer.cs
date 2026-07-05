using Serde;

namespace Serde.MsgPack;

partial class MsgPackWriter
{
    /// <summary>
    /// Serializes a custom type using the compact, positional representation: a msgpack
    /// array whose indices are the field ordinals (see <see cref="ISerdeInfo.GetFieldOrdinal"/>),
    /// matching MessagePack-CSharp's integer-key encoding. Skipped ordinals ("holes") and
    /// null fields are written as nil so positions stay aligned. A stack of frames supports
    /// nested compact types.
    /// </summary>
    private sealed class CompactSerializer(MsgPackWriter writer) : ITypeSerializer
    {
        private int[] _next = new int[8];
        private int[] _length = new int[8];
        private int _depth;

        /// <summary>Push a new frame for a compact type of the given array length.</summary>
        public void Begin(int length)
        {
            if (_depth == _next.Length)
            {
                Array.Resize(ref _next, _depth * 2);
                Array.Resize(ref _length, _depth * 2);
            }
            _next[_depth] = 0;
            _length[_depth] = length;
            _depth++;
        }

        public ISerializer WriteFieldStart(ISerdeInfo typeInfo, int index)
        {
            Advance(typeInfo, index);
            return writer;
        }

        public void WriteFieldEnd(ISerdeInfo typeInfo, int index, ISerializer serializer)
        {
            // No-op: positions are length-prefixed by the array header.
        }

        // Fill any skipped ordinals before this field with nil, then reserve this field's
        // position. The caller writes exactly one value, which lands at the field's ordinal.
        private void Advance(ISerdeInfo typeInfo, int index)
        {
            int top = _depth - 1;
            int ordinal = typeInfo.GetFieldOrdinal(index);
            int next = _next[top];
            while (next < ordinal)
            {
                writer.WriteNull();
                next++;
            }
            _next[top] = ordinal + 1;
        }

        public void End(ISerdeInfo typeInfo)
        {
            int top = _depth - 1;
            int next = _next[top];
            int length = _length[top];
            while (next < length)
            {
                writer.WriteNull();
                next++;
            }
            _depth--;
        }

        public void SkipValue(ISerdeInfo typeInfo, int index)
        {
            Advance(typeInfo, index);
            writer.WriteNull();
        }

        public void WriteValue<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
            where T : class?
        {
            Advance(typeInfo, index);
            serialize.Serialize(value, writer);
        }

        public void WriteEnum(ISerdeInfo typeInfo, int index, ISerdeInfo fieldInfo, int ordinal)
        {
            Advance(typeInfo, index);
            writer.WriteEnum(fieldInfo, ordinal);
        }

        public void WriteBool(ISerdeInfo typeInfo, int index, bool b)
        {
            Advance(typeInfo, index);
            writer.WriteBool(b);
        }

        public void WriteChar(ISerdeInfo typeInfo, int index, char c)
        {
            Advance(typeInfo, index);
            writer.WriteChar(c);
        }

        public void WriteU8(ISerdeInfo typeInfo, int index, byte b)
        {
            Advance(typeInfo, index);
            writer.WriteU8(b);
        }

        public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16)
        {
            Advance(typeInfo, index);
            writer.WriteU16(u16);
        }

        public void WriteU32(ISerdeInfo typeInfo, int index, uint u32)
        {
            Advance(typeInfo, index);
            writer.WriteU32(u32);
        }

        public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64)
        {
            Advance(typeInfo, index);
            writer.WriteU64(u64);
        }

        public void WriteU128(ISerdeInfo typeInfo, int index, UInt128 u128)
        {
            Advance(typeInfo, index);
            writer.WriteU128(u128);
        }

        public void WriteI128(ISerdeInfo typeInfo, int index, Int128 i128)
        {
            Advance(typeInfo, index);
            writer.WriteI128(i128);
        }

        public void WriteI8(ISerdeInfo typeInfo, int index, sbyte b)
        {
            Advance(typeInfo, index);
            writer.WriteI8(b);
        }

        public void WriteI16(ISerdeInfo typeInfo, int index, short i16)
        {
            Advance(typeInfo, index);
            writer.WriteI16(i16);
        }

        public void WriteI32(ISerdeInfo typeInfo, int index, int i32)
        {
            Advance(typeInfo, index);
            writer.WriteI32(i32);
        }

        public void WriteI64(ISerdeInfo typeInfo, int index, long i64)
        {
            Advance(typeInfo, index);
            writer.WriteI64(i64);
        }

        public void WriteF32(ISerdeInfo typeInfo, int index, float f)
        {
            Advance(typeInfo, index);
            writer.WriteF32(f);
        }

        public void WriteF64(ISerdeInfo typeInfo, int index, double d)
        {
            Advance(typeInfo, index);
            writer.WriteF64(d);
        }

        public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d)
        {
            Advance(typeInfo, index);
            writer.WriteDecimal(d);
        }

        public void WriteString(ISerdeInfo typeInfo, int index, string s)
        {
            Advance(typeInfo, index);
            writer.WriteString(s);
        }

        public void WriteNull(ISerdeInfo typeInfo, int index)
        {
            Advance(typeInfo, index);
            writer.WriteNull();
        }

        public void WriteDateTimeOffset(ISerdeInfo typeInfo, int index, DateTimeOffset dt)
        {
            Advance(typeInfo, index);
            writer.WriteDateTimeOffset(dt);
        }

        public void WriteDateTime(ISerdeInfo typeInfo, int index, DateTime dt)
        {
            Advance(typeInfo, index);
            writer.WriteDateTime(dt);
        }

        public void WriteBytes(ISerdeInfo typeInfo, int index, ReadOnlyMemory<byte> bytes)
        {
            Advance(typeInfo, index);
            writer.WriteBytes(bytes);
        }
    }
}
