
namespace Serde.MsgPack;

partial class MsgPackReader<TReader>
{
    private struct DeserializeCollection(MsgPackReader<TReader> deserializer, bool isDict, int length) : IDeserializeCollection
    {
        private int _index;
        int? IDeserializeCollection.SizeOpt => isDict switch {
            true => length / 2,
            false => length,
        };

        bool IDeserializeCollection.TryReadValue<T, D>(ISerdeInfo typeInfo, D d, out T next)
        {
            if (_index >= length)
            {
                next = default!;
                return false;
            }
            next = d.Deserialize(deserializer);
            _index++;
            return true;
        }
    }
}