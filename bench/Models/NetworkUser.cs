// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class NetworkUser : IGenericEquality<NetworkUser>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string site_name { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string site_url { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? user_id { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? reputation { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? account_id { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public UserType? user_type { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public User.BadgeCount badge_counts { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public DateTime? last_access_date { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public int? answer_count { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public int? question_count { get; set; }

        public bool Equals(NetworkUser obj)
        {
            return this.account_id.TrueEquals(obj.account_id)
                && this.answer_count.TrueEquals(obj.answer_count)
                && this.badge_counts.TrueEquals(obj.badge_counts)
                && this.creation_date.TrueEquals(obj.creation_date)
                && this.last_access_date.TrueEquals(obj.last_access_date)
                && this.question_count.TrueEquals(obj.question_count)
                && this.reputation.TrueEquals(obj.reputation)
                && this.site_name.TrueEqualsString(obj.site_name)
                && this.site_url.TrueEqualsString(obj.site_url)
                && this.user_id.TrueEquals(obj.user_id)
                && this.user_type.TrueEquals(obj.user_type);
        }
    }
}
