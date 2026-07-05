// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class MobileFeed : IGenericEquality<MobileFeed>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public List<MobileQuestion> hot_questions { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public List<MobileInboxItem> inbox_items { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public List<MobileQuestion> likely_to_answer_questions { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public List<MobileRepChange> reputation_events { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public List<MobileQuestion> cross_site_interesting_questions { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public List<MobileBadgeAward> badges { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public List<MobilePrivilege> earned_privileges { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public List<MobilePrivilege> upcoming_privileges { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public List<MobileCommunityBulletin> community_bulletins { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public List<MobileAssociationBonus> association_bonuses { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public List<MobileCareersJobAd> careers_job_ads { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public List<MobileBannerAd> banner_ads { get; set; }

        [MessagePack.Key(12)]
        [Serde.SerdeMemberOptions(Ordinal = 12)]
        public long? before { get; set; }

        [MessagePack.Key(13)]
        [Serde.SerdeMemberOptions(Ordinal = 13)]
        public long? since { get; set; }

        [MessagePack.Key(14)]
        [Serde.SerdeMemberOptions(Ordinal = 14)]
        public int? account_id { get; set; }

        [MessagePack.Key(15)]
        [Serde.SerdeMemberOptions(Ordinal = 15)]
        public MobileUpdateNotice update_notice { get; set; }

        public bool Equals(MobileFeed obj)
        {
            return this.account_id == obj.account_id
                && this.association_bonuses.TrueEqualsList(obj.association_bonuses)
                && this.badges.TrueEqualsList(obj.badges)
                && this.banner_ads.TrueEqualsList(obj.banner_ads)
                && this.before == obj.before
                && this.careers_job_ads.TrueEqualsList(obj.careers_job_ads)
                && this.community_bulletins.TrueEqualsList(obj.community_bulletins)
                && this.cross_site_interesting_questions.TrueEqualsList(
                    obj.cross_site_interesting_questions
                )
                && this.earned_privileges.TrueEqualsList(obj.earned_privileges)
                && this.hot_questions.TrueEqualsList(obj.hot_questions)
                && this.inbox_items.TrueEqualsList(obj.inbox_items)
                && this.likely_to_answer_questions.TrueEqualsList(obj.likely_to_answer_questions)
                && this.reputation_events.TrueEqualsList(obj.reputation_events)
                && this.since == obj.since
                && this.upcoming_privileges.TrueEqualsList(obj.upcoming_privileges)
                && this.update_notice.TrueEquals(obj.update_notice);
        }
    }

    public interface IMobileFeedBase<T> : IGenericEquality<T>
    {
        int? group_id { get; set; }

        long? added_date { get; set; }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobileQuestion : IMobileFeedBase<MobileQuestion>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? question_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public long? question_creation_date { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string title { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public long? last_activity_date { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public List<string> tags { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public string site { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public bool? is_deleted { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public bool? has_accepted_answer { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public int? answer_count { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public int? group_id { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public long? added_date { get; set; }

        public bool Equals(MobileQuestion obj)
        {
            return this.added_date == obj.added_date
                && this.answer_count == obj.answer_count
                && this.group_id == obj.group_id
                && this.has_accepted_answer == obj.has_accepted_answer
                && this.is_deleted == obj.is_deleted
                && this.last_activity_date == obj.last_activity_date
                && this.question_creation_date == obj.question_creation_date
                && this.question_id == obj.question_id
                && this.site == obj.site
                && this.tags.TrueEqualsString(obj.tags)
                && this.title == obj.title;
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobileRepChange : IMobileFeedBase<MobileRepChange>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string site { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string title { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string link { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? rep_change { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? group_id { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public long? added_date { get; set; }

        public bool Equals(MobileRepChange obj)
        {
            return this.added_date == obj.added_date
                && this.group_id == obj.group_id
                && this.link == obj.link
                && this.rep_change == obj.rep_change
                && this.site == obj.site
                && this.title == obj.title;
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobileInboxItem : IMobileFeedBase<MobileInboxItem>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? answer_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string body { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? comment_id { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public long? creation_date { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string item_type { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public string link { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? question_id { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public string title { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public string site { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public int? group_id { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public long? added_date { get; set; }

        public bool Equals(MobileInboxItem obj)
        {
            return this.added_date == obj.added_date
                && this.answer_id == obj.answer_id
                && this.body == obj.body
                && this.comment_id == obj.comment_id
                && this.creation_date == obj.creation_date
                && this.group_id == obj.group_id
                && this.item_type == obj.item_type
                && this.link == obj.link
                && this.question_id == obj.question_id
                && this.site == obj.site
                && this.title == obj.title;
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobileBadgeAward : IMobileFeedBase<MobileBadgeAward>
    {
        [Serde.GenerateSerde(AsUnderlying = true)]
        public enum BadgeRank : byte
        {
            bronze = 1,
            silver = 2,
            gold = 3,
        }

        [Serde.GenerateSerde(AsUnderlying = true)]
        public enum BadgeType
        {
            named = 1,
            tag_based = 2,
        }

        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string site { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string badge_name { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string badge_description { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? badge_id { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? post_id { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public string link { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public BadgeRank? rank { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public BadgeType? badge_type { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public int? group_id { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public long? added_date { get; set; }

        public bool Equals(MobileBadgeAward obj)
        {
            return this.added_date == obj.added_date
                && this.badge_description == obj.badge_description
                && this.badge_id == obj.badge_id
                && this.badge_name == obj.badge_name
                && this.badge_type == obj.badge_type
                && this.group_id == obj.group_id
                && this.link == obj.link
                && this.post_id == obj.post_id
                && this.rank == obj.rank
                && this.site == obj.site;
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobilePrivilege : IMobileFeedBase<MobilePrivilege>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string site { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string privilege_short_description { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string privilege_long_description { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? privilege_id { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? reputation_required { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public string link { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? group_id { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public long? added_date { get; set; }

        public bool Equals(MobilePrivilege obj)
        {
            return this.added_date == obj.added_date
                && this.group_id == obj.group_id
                && this.link == obj.link
                && this.privilege_id == obj.privilege_id
                && this.privilege_long_description == obj.privilege_long_description
                && this.privilege_short_description == obj.privilege_short_description
                && this.reputation_required == obj.reputation_required
                && this.site == obj.site;
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobileCommunityBulletin : IMobileFeedBase<MobileCommunityBulletin>
    {
        [Serde.GenerateSerde(AsUnderlying = true)]
        public enum CommunityBulletinType : byte
        {
            blog_post = 1,
            featured_meta_question = 2,
            upcoming_event = 3,
        }

        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string site { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string title { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string link { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public CommunityBulletinType? bulletin_type { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public long? begin_date { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public long? end_date { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public string custom_date_string { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public List<string> tags { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public bool? is_deleted { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public bool? has_accepted_answer { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public int? answer_count { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public bool? is_promoted { get; set; }

        [MessagePack.Key(12)]
        [Serde.SerdeMemberOptions(Ordinal = 12)]
        public int? group_id { get; set; }

        [MessagePack.Key(13)]
        [Serde.SerdeMemberOptions(Ordinal = 13)]
        public long? added_date { get; set; }

        public bool Equals(MobileCommunityBulletin obj)
        {
            return this.added_date == obj.added_date
                && this.answer_count == obj.answer_count
                && this.begin_date == obj.begin_date
                && this.bulletin_type == obj.bulletin_type
                && this.custom_date_string == obj.custom_date_string
                && this.end_date == obj.end_date
                && this.group_id == obj.group_id
                && this.has_accepted_answer == obj.has_accepted_answer
                && this.is_deleted == obj.is_deleted
                && this.is_promoted == obj.is_promoted
                && this.link == obj.link
                && this.site == obj.site
                && this.tags.TrueEqualsString(obj.tags)
                && this.title == obj.title;
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobileAssociationBonus : IMobileFeedBase<MobileAssociationBonus>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string site { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? amount { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? group_id { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public long? added_date { get; set; }

        public bool Equals(MobileAssociationBonus obj)
        {
            return this.added_date == obj.added_date
                && this.amount == obj.amount
                && this.group_id == obj.group_id
                && this.site == obj.site;
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobileCareersJobAd : IMobileFeedBase<MobileCareersJobAd>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? job_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string link { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string company_name { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public string location { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string title { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public int? group_id { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public long? added_date { get; set; }

        public bool Equals(MobileCareersJobAd obj)
        {
            return this.added_date == obj.added_date
                && this.company_name == obj.company_name
                && this.group_id == obj.group_id
                && this.job_id == obj.job_id
                && this.link == obj.link
                && this.location == obj.location
                && this.title == obj.title;
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobileBannerAd : IMobileFeedBase<MobileBannerAd>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string link { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public List<MobileBannerAdImage> images { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? group_id { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public long? added_date { get; set; }

        public bool Equals(MobileBannerAd obj)
        {
            return this.added_date == obj.added_date
                && this.group_id == obj.group_id
                && this.images.TrueEqualsList(obj.images)
                && this.link == obj.link;
        }

        [MessagePack.MessagePackObject]
        [Serde.GenerateSerde]
        public sealed partial class MobileBannerAdImage : IGenericEquality<MobileBannerAdImage>
        {
            [MessagePack.Key(0)]
            [Serde.SerdeMemberOptions(Ordinal = 0)]
            public string image_url { get; set; }

            [MessagePack.Key(1)]
            [Serde.SerdeMemberOptions(Ordinal = 1)]
            public int? width { get; set; }

            [MessagePack.Key(2)]
            [Serde.SerdeMemberOptions(Ordinal = 2)]
            public int? height { get; set; }

            public bool Equals(MobileBannerAdImage obj)
            {
                return this.height == obj.height
                    && this.image_url == obj.image_url
                    && this.width == obj.width;
            }
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public sealed partial class MobileUpdateNotice : IGenericEquality<MobileUpdateNotice>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public bool? should_update { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string message { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string minimum_supported_version { get; set; }

        public bool Equals(MobileUpdateNotice obj)
        {
            return this.message == obj.message
                && this.minimum_supported_version == obj.minimum_supported_version
                && this.should_update == obj.should_update;
        }
    }
}
