// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.



namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum BadgeRank : byte
    {
        bronze = 3,
        silver = 2,
        gold = 1,
    }

    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum BadgeType
    {
        named = 1,
        tag_based = 2,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Badge : IGenericEquality<Badge>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? badge_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public BadgeRank? rank { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string name { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public string description { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? award_count { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public BadgeType? badge_type { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public ShallowUser user { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public string link { get; set; }

        public bool Equals(Badge obj)
        {
            return
                this.award_count.TrueEquals(obj.award_count) &&
                this.badge_id.TrueEquals(obj.badge_id) &&
                this.badge_type.TrueEquals(obj.badge_type) &&
                this.description.TrueEqualsString(obj.description) &&
                this.link.TrueEqualsString(obj.link) &&
                this.name.TrueEqualsString(obj.name) &&
                this.rank.TrueEquals(obj.rank) &&
                this.user.TrueEquals(obj.user);
        }
    }
}
