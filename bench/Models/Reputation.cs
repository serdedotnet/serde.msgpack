// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum VoteType : byte
    {
        up_votes = 2,
        down_votes = 3,
        spam = 12,
        accepts = 1,
        bounties_won = 9,
        bounties_offered = 8,
        suggested_edits = 16,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Reputation : IGenericEquality<Reputation>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? user_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? post_id { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public PostType? post_type { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public VoteType? vote_type { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string title { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public string link { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? reputation_change { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public DateTime? on_date { get; set; }

        public bool Equals(Reputation obj)
        {
            return this.link.TrueEqualsString(obj.link)
                && this.on_date.TrueEquals(obj.on_date)
                && this.post_id.TrueEquals(obj.post_id)
                && this.post_type.TrueEquals(obj.post_type)
                && this.reputation_change.TrueEquals(obj.reputation_change)
                && this.title.TrueEqualsString(obj.title)
                && this.user_id.TrueEquals(obj.user_id)
                && this.vote_type.TrueEquals(obj.vote_type);
        }
    }
}
