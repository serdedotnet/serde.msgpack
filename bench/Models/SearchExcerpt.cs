// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Collections.Generic;

#pragma warning disable SA1300 // Element should begin with upper-case letter

namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum SearchExcerptItemType : byte
    {
        question = 1,
        answer = 2,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class SearchExcerpt : IGenericEquality<SearchExcerpt>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string title { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string excerpt { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public DateTime? community_owned_date { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public DateTime? locked_date { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public DateTime? last_activity_date { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public ShallowUser owner { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public ShallowUser last_activity_user { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public int? score { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public SearchExcerptItemType? item_type { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public string body { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public int? question_id { get; set; }

        [MessagePack.Key(12)]
        [Serde.SerdeMemberOptions(Ordinal = 12)]
        public bool? is_answered { get; set; }

        [MessagePack.Key(13)]
        [Serde.SerdeMemberOptions(Ordinal = 13)]
        public int? answer_count { get; set; }

        [MessagePack.Key(14)]
        [Serde.SerdeMemberOptions(Ordinal = 14)]
        public List<string> tags { get; set; }

        [MessagePack.Key(15)]
        [Serde.SerdeMemberOptions(Ordinal = 15)]
        public DateTime? closed_date { get; set; }

        [MessagePack.Key(16)]
        [Serde.SerdeMemberOptions(Ordinal = 16)]
        public int? answer_id { get; set; }

        [MessagePack.Key(17)]
        [Serde.SerdeMemberOptions(Ordinal = 17)]
        public bool? is_accepted { get; set; }

        public bool Equals(SearchExcerpt obj)
        {
            return
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.answer_id.TrueEquals(obj.answer_id) &&
                this.body.TrueEqualsString(obj.body) &&
                this.closed_date.TrueEquals(obj.closed_date) &&
                this.community_owned_date.TrueEquals(obj.community_owned_date) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.excerpt.TrueEqualsString(obj.excerpt) &&
                this.is_accepted.TrueEquals(obj.is_accepted) &&
                this.is_answered.TrueEquals(obj.is_answered) &&
                this.item_type.TrueEquals(obj.item_type) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_activity_user.TrueEquals(obj.last_activity_user) &&
                this.locked_date.TrueEquals(obj.locked_date) &&
                this.owner.TrueEquals(obj.owner) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.score.TrueEquals(obj.score) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title);
        }
    }
}
