// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Tag : IGenericEquality<Tag>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string name { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? count { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public bool? is_required { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public bool? is_moderator_only { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? user_id { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public bool? has_synonyms { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public DateTime? last_activity_date { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public List<string> synonyms { get; set; }

        public bool Equals(Tag obj)
        {
            return this.count.TrueEquals(obj.count)
                && this.has_synonyms.TrueEquals(obj.has_synonyms)
                && this.is_moderator_only.TrueEquals(obj.is_moderator_only)
                && this.is_required.TrueEquals(obj.is_required)
                && this.last_activity_date.TrueEquals(obj.last_activity_date)
                && this.name.TrueEqualsString(obj.name)
                && this.synonyms.TrueEqualsString(obj.synonyms)
                && this.user_id.TrueEquals(obj.user_id);
        }
    }
}
