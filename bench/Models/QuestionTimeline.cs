// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum QuestionTimelineAction : byte
    {
        question = 1,
        answer = 2,
        comment = 3,
        unaccepted_answer = 4,
        accepted_answer = 5,
        vote_aggregate = 6,
        revision = 7,
        post_state_changed = 8,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class QuestionTimeline : IGenericEquality<QuestionTimeline>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public QuestionTimelineAction? timeline_type { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? question_id { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? post_id { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? comment_id { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string revision_guid { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public int? up_vote_count { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? down_vote_count { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public ShallowUser user { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public ShallowUser owner { get; set; }

        public bool Equals(QuestionTimeline obj)
        {
            return this.comment_id.TrueEquals(obj.comment_id)
                && this.creation_date.TrueEquals(obj.creation_date)
                && this.down_vote_count.TrueEquals(obj.down_vote_count)
                && this.owner.TrueEquals(obj.owner)
                && this.post_id.TrueEquals(obj.post_id)
                && this.question_id.TrueEquals(obj.question_id)
                && this.revision_guid.TrueEqualsString(obj.revision_guid)
                && this.timeline_type.TrueEquals(obj.timeline_type)
                && this.up_vote_count.TrueEquals(obj.up_vote_count)
                && this.user.TrueEquals(obj.user);
        }
    }
}
