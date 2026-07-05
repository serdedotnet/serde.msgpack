// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable IDE1006
#pragma warning disable SA1516

namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum UserType : byte
    {
        unregistered = 2,
        registered = 3,
        moderator = 4,
        does_not_exist = 255,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class ShallowUser : IGenericEquality<ShallowUser>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? user_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string display_name { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? reputation { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public UserType? user_type { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string profile_image { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public string link { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? accept_rate { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public User.BadgeCount badge_counts { get; set; }

        public bool Equals(ShallowUser obj)
        {
            return this.accept_rate.TrueEquals(obj.accept_rate)
                && this.badge_counts.TrueEquals(obj.badge_counts)
                && this.display_name.TrueEqualsString(obj.display_name)
                && this.link.TrueEqualsString(obj.link)
                && this.profile_image.TrueEqualsString(obj.profile_image)
                && this.reputation.TrueEquals(obj.reputation)
                && this.user_id.TrueEquals(obj.user_id)
                && this.user_type.TrueEquals(obj.user_type);
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class ShallowUser2 : IGenericEquality<ShallowUser2>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? user_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string display_name { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? reputation { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public UserType? user_type { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string profile_image { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public string link { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? accept_rate { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public User.BadgeCount2 badge_counts { get; set; }

        public bool Equals(ShallowUser2 obj)
        {
            return this.accept_rate.TrueEquals(obj.accept_rate)
                && this.badge_counts.TrueEquals(obj.badge_counts)
                && this.display_name.TrueEqualsString(obj.display_name)
                && this.link.TrueEqualsString(obj.link)
                && this.profile_image.TrueEqualsString(obj.profile_image)
                && this.reputation.TrueEquals(obj.reputation)
                && this.user_id.TrueEquals(obj.user_id)
                && this.user_type.TrueEquals(obj.user_type);
        }
    }
}
