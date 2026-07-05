// Head-to-head benchmarks of Serde.MsgPack vs MessagePack-CSharp over the
// StackExchange API model set (ported from MessagePack-CSharp's SerializerBenchmark).
// All models use the compact positional-array representation (integer keys /
// explicit Serde ordinals) on both sides for a fair, like-for-like comparison.

using BenchmarkDotNet.Attributes;
using Benchmark.Models;
using MessagePack;
using Serde;

namespace Benchmarks
{
    [ShortRunJob]
    [GenericTypeArguments(typeof(AccessToken))]
    [GenericTypeArguments(typeof(AccountMerge))]
    [GenericTypeArguments(typeof(Answer))]
    [GenericTypeArguments(typeof(Answer2))]
    [GenericTypeArguments(typeof(Badge))]
    [GenericTypeArguments(typeof(Comment))]
    [GenericTypeArguments(typeof(Comment2))]
    [GenericTypeArguments(typeof(Error))]
    [GenericTypeArguments(typeof(Event))]
    [GenericTypeArguments(typeof(InboxItem))]
    [GenericTypeArguments(typeof(Info))]
    [GenericTypeArguments(typeof(MobileFeed))]
    [GenericTypeArguments(typeof(NetworkUser))]
    [GenericTypeArguments(typeof(Notification))]
    [GenericTypeArguments(typeof(Post))]
    [GenericTypeArguments(typeof(Privilege))]
    [GenericTypeArguments(typeof(Question))]
    [GenericTypeArguments(typeof(QuestionTimeline))]
    [GenericTypeArguments(typeof(Reputation))]
    [GenericTypeArguments(typeof(ReputationHistory))]
    [GenericTypeArguments(typeof(Revision))]
    [GenericTypeArguments(typeof(SearchExcerpt))]
    [GenericTypeArguments(typeof(ShallowUser))]
    [GenericTypeArguments(typeof(ShallowUser2))]
    [GenericTypeArguments(typeof(SuggestedEdit))]
    [GenericTypeArguments(typeof(Tag))]
    [GenericTypeArguments(typeof(TagScore))]
    [GenericTypeArguments(typeof(TagSynonym))]
    [GenericTypeArguments(typeof(TagWiki))]
    [GenericTypeArguments(typeof(TopTag))]
    [GenericTypeArguments(typeof(User))]
    [GenericTypeArguments(typeof(UserTimeline))]
    [GenericTypeArguments(typeof(WritePermission))]
    public class SerializeModel<T>
        where T : Serde.ISerializeProvider<T>
    {
        private T _value = default!;
        private readonly ISerialize<T> _proxy = T.Instance;

        [GlobalSetup]
        public void Setup() => _value = ModelData.Sample<T>();

        [Benchmark]
        public byte[] MessagePack() => MessagePackSerializer.Serialize(_value);

        [Benchmark]
        public byte[] SerdeMsgPack() => Serde.MsgPack.MsgPackSerializer.Serialize(_value, _proxy);
    }

    [ShortRunJob]
    [GenericTypeArguments(typeof(AccessToken))]
    [GenericTypeArguments(typeof(AccountMerge))]
    [GenericTypeArguments(typeof(Answer))]
    [GenericTypeArguments(typeof(Answer2))]
    [GenericTypeArguments(typeof(Badge))]
    [GenericTypeArguments(typeof(Comment))]
    [GenericTypeArguments(typeof(Comment2))]
    [GenericTypeArguments(typeof(Error))]
    [GenericTypeArguments(typeof(Event))]
    [GenericTypeArguments(typeof(InboxItem))]
    [GenericTypeArguments(typeof(Info))]
    [GenericTypeArguments(typeof(MobileFeed))]
    [GenericTypeArguments(typeof(NetworkUser))]
    [GenericTypeArguments(typeof(Notification))]
    [GenericTypeArguments(typeof(Post))]
    [GenericTypeArguments(typeof(Privilege))]
    [GenericTypeArguments(typeof(Question))]
    [GenericTypeArguments(typeof(QuestionTimeline))]
    [GenericTypeArguments(typeof(Reputation))]
    [GenericTypeArguments(typeof(ReputationHistory))]
    [GenericTypeArguments(typeof(Revision))]
    [GenericTypeArguments(typeof(SearchExcerpt))]
    [GenericTypeArguments(typeof(ShallowUser))]
    [GenericTypeArguments(typeof(ShallowUser2))]
    [GenericTypeArguments(typeof(SuggestedEdit))]
    [GenericTypeArguments(typeof(Tag))]
    [GenericTypeArguments(typeof(TagScore))]
    [GenericTypeArguments(typeof(TagSynonym))]
    [GenericTypeArguments(typeof(TagWiki))]
    [GenericTypeArguments(typeof(TopTag))]
    [GenericTypeArguments(typeof(User))]
    [GenericTypeArguments(typeof(UserTimeline))]
    [GenericTypeArguments(typeof(WritePermission))]
    public class DeserializeModel<T>
        where T : Serde.ISerializeProvider<T>, Serde.IDeserializeProvider<T>
    {
        private byte[] _mpBytes = null!;
        private byte[] _serdeBytes = null!;
        private readonly IDeserialize<T> _proxy = DeserializeProvider.GetDeserialize<T>();

        [GlobalSetup]
        public void Setup()
        {
            var value = ModelData.Sample<T>();
            // Both libraries now produce byte-identical output, but we still let
            // each decode the bytes produced by its own serializer for symmetry.
            _mpBytes = MessagePackSerializer.Serialize(value);
            _serdeBytes = Serde.MsgPack.MsgPackSerializer.Serialize(value, Providers.Ser<T>());
        }

        [Benchmark]
        public T? MessagePack() => MessagePackSerializer.Deserialize<T>(_mpBytes);

        [Benchmark]
        public T SerdeMsgPack() => Serde.MsgPack.MsgPackSerializer.Deserialize<T, IDeserialize<T>>(_serdeBytes, _proxy);
    }

    /// <summary>
    /// Serializes an array of <c>User</c> so the per-call serializer setup is
    /// amortized across many elements, isolating steady-state per-element cost
    /// from one-time spin-up overhead.
    /// </summary>
    [ShortRunJob]
    public class SerializeUserArray
    {
        private const int Count = 100;
        private User[] _value = default!;
        private readonly ISerialize<User[]> _proxy = ArrayProxy.Ser<User, User>.Instance;

        [GlobalSetup]
        public void Setup()
        {
            var sample = ModelData.Sample<User>();
            _value = new User[Count];
            for (int i = 0; i < Count; i++)
            {
                _value[i] = sample;
            }
        }

        [Benchmark]
        public byte[] MessagePack() => MessagePackSerializer.Serialize(_value);

        [Benchmark]
        public byte[] SerdeMsgPack() => Serde.MsgPack.MsgPackSerializer.Serialize(_value, _proxy);
    }

    /// <summary>
    /// Deserializes an array of <c>User</c>, amortizing per-call setup across many
    /// elements (counterpart to <see cref="SerializeUserArray"/>).
    /// </summary>
    [ShortRunJob]
    public class DeserializeUserArray
    {
        private const int Count = 100;
        private byte[] _mpBytes = null!;
        private byte[] _serdeBytes = null!;
        private readonly IDeserialize<User[]> _proxy = ArrayProxy.De<User, User>.Instance;

        [GlobalSetup]
        public void Setup()
        {
            var sample = ModelData.Sample<User>();
            var value = new User[Count];
            for (int i = 0; i < Count; i++)
            {
                value[i] = sample;
            }
            _mpBytes = MessagePackSerializer.Serialize(value);
            _serdeBytes = Serde.MsgPack.MsgPackSerializer.Serialize(value, ArrayProxy.Ser<User, User>.Instance);
        }

        [Benchmark]
        public User[]? MessagePack() => MessagePackSerializer.Deserialize<User[]>(_mpBytes);

        [Benchmark]
        public User[] SerdeMsgPack() => Serde.MsgPack.MsgPackSerializer.Deserialize<User[], IDeserialize<User[]>>(_serdeBytes, _proxy);
    }

    /// <summary>
    /// Single-constraint helpers so the proxy <c>Instance</c> can be obtained
    /// without ambiguity when a type implements both provider interfaces.
    /// </summary>
    internal static class Providers
    {
        public static ISerialize<T> Ser<T>() where T : ISerializeProvider<T> => T.Instance;
        public static IDeserialize<T> Deser<T>() where T : IDeserializeProvider<T> => T.Instance;
    }
}