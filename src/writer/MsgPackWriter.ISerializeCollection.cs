
namespace Serde.MsgPack;

partial class MsgPackWriter : ISerializeCollection
{
    void ISerializeCollection.End(ISerdeInfo typeInfo)
    {
        // No action needed, all collections are length-prefixed
    }

    void ISerializeCollection.WriteElement<T, U>(T value, U serialize)
    {
        serialize.Serialize(value, this);
    }
}