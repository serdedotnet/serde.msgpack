// Cross-library round-trip correctness check for the ported StackExchange models.
// For each model: serialize+deserialize with each library independently and assert
// the value survives the round-trip (IGenericEquality), then report whether the two
// libraries produced byte-identical output (they diverge when DateTime fields are
// present: MessagePack uses the msgpack timestamp ext, Serde an ISO-8601 string).

using Benchmark;
using Benchmark.Models;
using MessagePack;
using Serde;

namespace Benchmarks
{
    internal static class CorrectnessCheck
    {
        public static void Run()
        {
            int identical = 0,
                diverged = 0;
            Check<AccessToken>();
            Check<AccountMerge>();
            Check<Answer>();
            Check<Answer2>();
            Check<Badge>();
            Check<Comment>();
            Check<Comment2>();
            Check<Error>();
            Check<Event>();
            Check<InboxItem>();
            Check<Info>();
            Check<MobileFeed>();
            Check<NetworkUser>();
            Check<Notification>();
            Check<Post>();
            Check<Privilege>();
            Check<Question>();
            Check<QuestionTimeline>();
            Check<Reputation>();
            Check<ReputationHistory>();
            Check<Revision>();
            Check<SearchExcerpt>();
            Check<ShallowUser>();
            Check<ShallowUser2>();
            Check<SuggestedEdit>();
            Check<Tag>();
            Check<TagScore>();
            Check<TagSynonym>();
            Check<TagWiki>();
            Check<TopTag>();
            Check<User>();
            Check<UserTimeline>();
            Check<WritePermission>();
            Console.WriteLine();
            Console.WriteLine(
                $"Round-trip OK for all 33 models (FlagOption excluded: Serde static-init cycle on self-recursive List<FlagOption>). Byte-identical: {identical}, diverged: {diverged}."
            );

            void Check<T>()
                where T : ISerializeProvider<T>, IDeserializeProvider<T>, IGenericEquality<T>
            {
                var value = ModelData.Sample<T>();

                var mpBytes = MessagePackSerializer.Serialize(value);
                var mpBack = MessagePackSerializer.Deserialize<T>(mpBytes);
                if (!value.Equals(mpBack))
                    throw new InvalidOperationException(
                        $"MessagePack round-trip mismatch for {typeof(T).Name}"
                    );

                var serdeBytes = Serde.MsgPack.MsgPackSerializer.Serialize(
                    value,
                    Providers.Ser<T>()
                );
                var serdeBack = Serde.MsgPack.MsgPackSerializer.Deserialize<T, IDeserialize<T>>(
                    serdeBytes,
                    Providers.Deser<T>()
                );
                if (!value.Equals(serdeBack))
                    throw new InvalidOperationException(
                        $"Serde round-trip mismatch for {typeof(T).Name}"
                    );

                bool same = mpBytes.AsSpan().SequenceEqual(serdeBytes);
                if (same)
                    identical++;
                else
                    diverged++;
                Console.WriteLine(
                    $"  {typeof(T).Name, -20} mp={mpBytes.Length, 5}B serde={serdeBytes.Length, 5}B  {(same ? "identical" : "diverged")}"
                );
            }
        }
    }
}
