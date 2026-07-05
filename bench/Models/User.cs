// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#pragma warning disable IDE1006
#pragma warning disable SA1516

using System;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class User : IGenericEquality<User>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? user_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public UserType? user_type { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public string display_name { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string profile_image { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public int? reputation { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? reputation_change_day { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public int? reputation_change_week { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public int? reputation_change_month { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public int? reputation_change_quarter { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public int? reputation_change_year { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public int? age { get; set; }

        [MessagePack.Key(12)]
        [Serde.SerdeMemberOptions(Ordinal = 12)]
        public DateTime? last_access_date { get; set; }

        [MessagePack.Key(13)]
        [Serde.SerdeMemberOptions(Ordinal = 13)]
        public DateTime? last_modified_date { get; set; }

        [MessagePack.Key(14)]
        [Serde.SerdeMemberOptions(Ordinal = 14)]
        public bool? is_employee { get; set; }

        [MessagePack.Key(15)]
        [Serde.SerdeMemberOptions(Ordinal = 15)]
        public string link { get; set; }

        [MessagePack.Key(16)]
        [Serde.SerdeMemberOptions(Ordinal = 16)]
        public string website_url { get; set; }

        [MessagePack.Key(17)]
        [Serde.SerdeMemberOptions(Ordinal = 17)]
        public string location { get; set; }

        [MessagePack.Key(18)]
        [Serde.SerdeMemberOptions(Ordinal = 18)]
        public int? account_id { get; set; }

        [MessagePack.Key(19)]
        [Serde.SerdeMemberOptions(Ordinal = 19)]
        public DateTime? timed_penalty_date { get; set; }

        [MessagePack.Key(20)]
        [Serde.SerdeMemberOptions(Ordinal = 20)]
        public BadgeCount badge_counts { get; set; }

        [MessagePack.Key(21)]
        [Serde.SerdeMemberOptions(Ordinal = 21)]
        public int? question_count { get; set; }

        [MessagePack.Key(22)]
        [Serde.SerdeMemberOptions(Ordinal = 22)]
        public int? answer_count { get; set; }

        [MessagePack.Key(23)]
        [Serde.SerdeMemberOptions(Ordinal = 23)]
        public int? up_vote_count { get; set; }

        [MessagePack.Key(24)]
        [Serde.SerdeMemberOptions(Ordinal = 24)]
        public int? down_vote_count { get; set; }

        [MessagePack.Key(25)]
        [Serde.SerdeMemberOptions(Ordinal = 25)]
        public string about_me { get; set; }

        [MessagePack.Key(26)]
        [Serde.SerdeMemberOptions(Ordinal = 26)]
        public int? view_count { get; set; }

        [MessagePack.Key(27)]
        [Serde.SerdeMemberOptions(Ordinal = 27)]
        public int? accept_rate { get; set; }

        public bool Equals(User obj)
        {
            return
                this.about_me.TrueEqualsString(obj.about_me) &&
                this.accept_rate.TrueEquals(obj.accept_rate) &&
                this.account_id.TrueEquals(obj.account_id) &&
                this.age.TrueEquals(obj.age) &&
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.badge_counts.TrueEquals(obj.badge_counts) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.display_name.TrueEqualsString(obj.display_name) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.is_employee.TrueEquals(obj.is_employee) &&
                this.last_access_date.TrueEquals(obj.last_access_date) &&
                this.last_modified_date.TrueEquals(obj.last_modified_date) &&
                this.link.TrueEqualsString(obj.link) &&
                this.location.TrueEqualsString(obj.location) &&
                this.profile_image.TrueEqualsString(obj.profile_image) &&
                this.question_count.TrueEquals(obj.question_count) &&
                this.reputation.TrueEquals(obj.reputation) &&
                this.reputation_change_day.TrueEquals(obj.reputation_change_day) &&
                this.reputation_change_month.TrueEquals(obj.reputation_change_month) &&
                this.reputation_change_quarter.TrueEquals(obj.reputation_change_quarter) &&
                this.reputation_change_week.TrueEquals(obj.reputation_change_week) &&
                this.reputation_change_year.TrueEquals(obj.reputation_change_year) &&
                this.timed_penalty_date.TrueEquals(obj.timed_penalty_date) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.user_id.TrueEquals(obj.user_id) &&
                this.user_type.TrueEquals(obj.user_type) &&
                this.view_count.TrueEquals(obj.view_count) &&
                this.website_url.TrueEqualsString(obj.website_url);
        }

        [MessagePack.MessagePackObject]
        [Serde.GenerateSerde]
        public partial class BadgeCount : IGenericEquality<BadgeCount>
        {
            [MessagePack.Key(0)]
            [Serde.SerdeMemberOptions(Ordinal = 0)]
            public int? gold { get; set; }

            [MessagePack.Key(1)]
            [Serde.SerdeMemberOptions(Ordinal = 1)]
            public int? silver { get; set; }

            [MessagePack.Key(2)]
            [Serde.SerdeMemberOptions(Ordinal = 2)]
            public int? bronze { get; set; }

            public bool Equals(BadgeCount obj)
            {
                return
                    this.bronze.TrueEquals(obj.bronze) &&
                    this.silver.TrueEquals(obj.silver) &&
                    this.gold.TrueEquals(obj.gold);
            }
        }

        [MessagePack.MessagePackObject]
        [Serde.GenerateSerde]
        public partial class BadgeCount2 : IGenericEquality<BadgeCount2>
        {
            [MessagePack.Key(0)]
            [Serde.SerdeMemberOptions(Ordinal = 0)]
            public int? gold { get; set; }
            [MessagePack.Key(1)]
            [Serde.SerdeMemberOptions(Ordinal = 1)]
            public int? silver { get; set; }
            [MessagePack.Key(2)]
            [Serde.SerdeMemberOptions(Ordinal = 2)]
            public int? bronze { get; set; }

            public bool Equals(BadgeCount2 obj)
            {
                return
                    this.bronze.TrueEquals(obj.bronze) &&
                    this.silver.TrueEquals(obj.silver) &&
                    this.gold.TrueEquals(obj.gold);
            }
        }
    }
}
