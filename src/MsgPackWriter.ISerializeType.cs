
using Serde;

namespace Serde.MsgPack;

partial class MsgPackWriter : ISerializeType
{
    void ISerializeType.End()
    {
        // No action needed, all collections are length-prefixed
    }

    void ISerializeType.SerializeField<T, U>(ISerdeInfo typeInfo, int index, T value, U serialize)
    {
        serialize.Serialize(value, this);
    }
}