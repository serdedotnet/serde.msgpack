// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum UserTimelineType : byte
    {
        commented = 1,
        asked = 2,
        answered = 3,
        badge = 4,
        revision = 5,
        accepted = 6,
        reviewed = 7,
        suggested = 8,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class UserTimeline : IGenericEquality<UserTimeline>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public PostType? post_type { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public UserTimelineType? timeline_type { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? user_id { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? post_id { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public int? comment_id { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? suggested_edit_id { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public int? badge_id { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public string title { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public string detail { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public string link { get; set; }

        public bool Equals(UserTimeline obj)
        {
            return this.badge_id.TrueEquals(obj.badge_id)
                && this.comment_id.TrueEquals(obj.comment_id)
                && this.creation_date.TrueEquals(obj.creation_date)
                && this.detail.TrueEqualsString(obj.detail)
                && this.link.TrueEqualsString(obj.link)
                && this.post_id.TrueEquals(obj.post_id)
                && this.post_type.TrueEquals(obj.post_type)
                && this.suggested_edit_id.TrueEquals(obj.suggested_edit_id)
                && this.timeline_type.TrueEquals(obj.timeline_type)
                && this.title.TrueEqualsString(obj.title)
                && this.user_id.TrueEquals(obj.user_id);
        }
    }
}
