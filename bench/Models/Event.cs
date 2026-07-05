// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;

namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum EventType : byte
    {
        question_posted = 1,
        answer_posted = 2,
        comment_posted = 3,
        post_edited = 4,
        user_created = 5,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Event : IGenericEquality<Event>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public EventType? event_type { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? event_id { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public string link { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string excerpt { get; set; }

        public bool Equals(Event obj)
        {
            return
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.event_id.TrueEquals(obj.event_id) &&
                this.event_type.TrueEquals(obj.event_type) &&
                this.excerpt.TrueEqualsString(obj.excerpt) &&
                this.link.TrueEqualsString(obj.link);
        }
    }
}
