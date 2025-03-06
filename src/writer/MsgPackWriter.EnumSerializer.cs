
using System.Runtime.CompilerServices;

namespace Serde.MsgPack;

partial class MsgPackWriter
{
    private sealed class EnumSerializer(MsgPackWriter writer) : ISerializeType
    {
        public void End(ISerdeInfo info)
        {
            // Enums aren't nested, so no action needed
        }

        private void ThrowInvalidEnum([CallerMemberName] string memberName = "<error>")
        {
            throw new InvalidOperationException($"EnumSerializer cannot be used for non-enum types. Called from {memberName}.");
        }

        public void WriteBool(ISerdeInfo typeInfo, int index, bool b) => ThrowInvalidEnum();
        public void WriteByte(ISerdeInfo typeInfo, int index, byte b) => writer.WriteByte(b);
        public void WriteChar(ISerdeInfo typeInfo, int index, char c) => ThrowInvalidEnum();
        public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d) => ThrowInvalidEnum();
        public void WriteDouble(ISerdeInfo typeInfo, int index, double d) => ThrowInvalidEnum();
        public void WriteField<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize)
            where T : class?
            => ThrowInvalidEnum();
        public void WriteFloat(ISerdeInfo typeInfo, int index, float f) => ThrowInvalidEnum();
        public void WriteI16(ISerdeInfo typeInfo, int index, short i16) => writer.WriteI16(i16);
        public void WriteI32(ISerdeInfo typeInfo, int index, int i32) => writer.WriteI32(i32);
        public void WriteI64(ISerdeInfo typeInfo, int index, long i64) => writer.WriteI64(i64);
        public void WriteNull(ISerdeInfo typeInfo, int index) => ThrowInvalidEnum();
        public void WriteSByte(ISerdeInfo typeInfo, int index, sbyte b) => writer.WriteSByte(b);
        public void WriteString(ISerdeInfo typeInfo, int index, string s) => ThrowInvalidEnum();
        public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16) => writer.WriteU16(u16);
        public void WriteU32(ISerdeInfo typeInfo, int index, uint u32) => writer.WriteU32(u32);
        public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64) => writer.WriteU64(u64);
    }
}