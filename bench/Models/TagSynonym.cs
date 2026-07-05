// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class TagSynonym : IGenericEquality<TagSynonym>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string from_tag { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string to_tag { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? applied_count { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public DateTime? last_applied_date { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public DateTime? creation_date { get; set; }

        public bool Equals(TagSynonym obj)
        {
            return
                this.applied_count.TrueEquals(obj.applied_count) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.from_tag.TrueEqualsString(obj.from_tag) &&
                this.last_applied_date.TrueEquals(obj.last_applied_date) &&
                this.to_tag.TrueEqualsString(obj.to_tag);
        }
    }
}
