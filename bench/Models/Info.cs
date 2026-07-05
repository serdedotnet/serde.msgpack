// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Collections.Generic;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Info : IGenericEquality<Info>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? total_questions { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? total_unanswered { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? total_accepted { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? total_answers { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public decimal? questions_per_minute { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public decimal? answers_per_minute { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? total_comments { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public int? total_votes { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public int? total_badges { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public decimal? badges_per_minute { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public int? total_users { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public int? new_active_users { get; set; }

        [MessagePack.Key(12)]
        [Serde.SerdeMemberOptions(Ordinal = 12)]
        public string api_revision { get; set; }

        [MessagePack.Key(13)]
        [Serde.SerdeMemberOptions(Ordinal = 13)]
        public Site site { get; set; }

        public bool Equals(Info obj)
        {
            return
                this.answers_per_minute.TrueEquals(obj.answers_per_minute) &&
                this.api_revision.TrueEqualsString(obj.api_revision) &&
                this.badges_per_minute.TrueEquals(obj.badges_per_minute) &&
                this.new_active_users.TrueEquals(obj.new_active_users) &&
                this.questions_per_minute.TrueEquals(obj.questions_per_minute) &&
                this.site.TrueEquals(obj.site) &&
                this.total_accepted.TrueEquals(obj.total_accepted) &&
                this.total_answers.TrueEquals(obj.total_answers) &&
                this.total_badges.TrueEquals(obj.total_badges) &&
                this.total_comments.TrueEquals(obj.total_comments) &&
                this.total_questions.TrueEquals(obj.total_questions) &&
                this.total_unanswered.TrueEquals(obj.total_unanswered) &&
                this.total_users.TrueEquals(obj.total_users) &&
                this.total_votes.TrueEquals(obj.total_votes);
        }

        [MessagePack.MessagePackObject]
        [Serde.GenerateSerde]
        public partial class Site : IGenericEquality<Site>
        {
            [Serde.GenerateSerde(AsUnderlying = true)]
            public enum SiteState
            {
                normal,
                closed_beta,
                open_beta,
                linked_meta,
            }

            [MessagePack.Key(0)]
            [Serde.SerdeMemberOptions(Ordinal = 0)]
            public string site_type { get; set; }

            [MessagePack.Key(1)]
            [Serde.SerdeMemberOptions(Ordinal = 1)]
            public string name { get; set; }

            [MessagePack.Key(2)]
            [Serde.SerdeMemberOptions(Ordinal = 2)]
            public string logo_url { get; set; }

            [MessagePack.Key(3)]
            [Serde.SerdeMemberOptions(Ordinal = 3)]
            public string api_site_parameter { get; set; }

            [MessagePack.Key(4)]
            [Serde.SerdeMemberOptions(Ordinal = 4)]
            public string site_url { get; set; }

            [MessagePack.Key(5)]
            [Serde.SerdeMemberOptions(Ordinal = 5)]
            public string audience { get; set; }

            [MessagePack.Key(6)]
            [Serde.SerdeMemberOptions(Ordinal = 6)]
            public string icon_url { get; set; }

            [MessagePack.Key(7)]
            [Serde.SerdeMemberOptions(Ordinal = 7)]
            public List<string> aliases { get; set; }

            [MessagePack.Key(8)]
            [Serde.SerdeMemberOptions(Ordinal = 8)]
            public SiteState? site_state { get; set; }

            [MessagePack.Key(9)]
            [Serde.SerdeMemberOptions(Ordinal = 9)]
            public Styling styling { get; set; }

            [MessagePack.Key(10)]
            [Serde.SerdeMemberOptions(Ordinal = 10)]
            public DateTime? closed_beta_date { get; set; }

            [MessagePack.Key(11)]
            [Serde.SerdeMemberOptions(Ordinal = 11)]
            public DateTime? open_beta_date { get; set; }

            [MessagePack.Key(12)]
            [Serde.SerdeMemberOptions(Ordinal = 12)]
            public DateTime? launch_date { get; set; }

            [MessagePack.Key(13)]
            [Serde.SerdeMemberOptions(Ordinal = 13)]
            public string favicon_url { get; set; }

            [MessagePack.Key(14)]
            [Serde.SerdeMemberOptions(Ordinal = 14)]
            public List<RelatedSite> related_sites { get; set; }

            [MessagePack.Key(15)]
            [Serde.SerdeMemberOptions(Ordinal = 15)]
            public string twitter_account { get; set; }

            [MessagePack.Key(16)]
            [Serde.SerdeMemberOptions(Ordinal = 16)]
            public List<string> markdown_extensions { get; set; }

            [MessagePack.Key(17)]
            [Serde.SerdeMemberOptions(Ordinal = 17)]
            public string high_resolution_icon_url { get; set; }

            public bool Equals(Site obj)
            {
                return
                    this.aliases.TrueEqualsString(obj.aliases) &&
                    this.api_site_parameter.TrueEqualsString(obj.api_site_parameter) &&
                    this.audience.TrueEqualsString(obj.audience) &&
                    this.closed_beta_date.TrueEquals(obj.closed_beta_date) &&
                    this.favicon_url.TrueEqualsString(obj.favicon_url) &&
                    this.high_resolution_icon_url.TrueEqualsString(obj.high_resolution_icon_url) &&
                    this.icon_url.TrueEqualsString(obj.icon_url) &&
                    this.launch_date.TrueEquals(obj.launch_date) &&
                    this.logo_url.TrueEqualsString(obj.logo_url) &&
                    this.markdown_extensions.TrueEqualsString(obj.markdown_extensions) &&
                    this.name.TrueEqualsString(obj.name) &&
                    this.open_beta_date.TrueEquals(obj.open_beta_date) &&
                    this.related_sites.TrueEqualsList(obj.related_sites) &&
                    this.site_state.TrueEquals(obj.site_state) &&
                    this.site_type.TrueEqualsString(obj.site_type) &&
                    this.site_url.TrueEqualsString(obj.site_url) &&
                    this.styling.TrueEquals(obj.styling) &&
                    this.twitter_account.TrueEqualsString(obj.twitter_account);
            }

            [MessagePack.MessagePackObject]
            [Serde.GenerateSerde]
            public partial class Styling : IGenericEquality<Styling>
            {
                [MessagePack.Key(0)]
                [Serde.SerdeMemberOptions(Ordinal = 0)]
                public string link_color { get; set; }

                [MessagePack.Key(1)]
                [Serde.SerdeMemberOptions(Ordinal = 1)]
                public string tag_foreground_color { get; set; }

                [MessagePack.Key(2)]
                [Serde.SerdeMemberOptions(Ordinal = 2)]
                public string tag_background_color { get; set; }

                public bool Equals(Styling obj)
                {
                    return
                        this.link_color.TrueEqualsString(obj.link_color) &&
                        this.tag_background_color.TrueEqualsString(obj.tag_background_color) &&
                        this.tag_foreground_color.TrueEqualsString(obj.tag_foreground_color);
                }
            }
        }

        [MessagePack.MessagePackObject]
        [Serde.GenerateSerde]
        public partial class RelatedSite : IGenericEquality<RelatedSite>
        {
            [Serde.GenerateSerde(AsUnderlying = true)]
            public enum SiteRelation
            {
                parent,
                meta,
                chat,
            }

            [MessagePack.Key(0)]
            [Serde.SerdeMemberOptions(Ordinal = 0)]
            public string name { get; set; }

            [MessagePack.Key(1)]
            [Serde.SerdeMemberOptions(Ordinal = 1)]
            public string site_url { get; set; }

            [MessagePack.Key(2)]
            [Serde.SerdeMemberOptions(Ordinal = 2)]
            public SiteRelation? relation { get; set; }

            [MessagePack.Key(3)]
            [Serde.SerdeMemberOptions(Ordinal = 3)]
            public string api_site_parameter { get; set; }

            public bool Equals(RelatedSite obj)
            {
                return
                    this.name.TrueEqualsString(obj.name) &&
                    this.relation.TrueEquals(obj.relation) &&
                    this.api_site_parameter.TrueEqualsString(obj.api_site_parameter);
            }
        }
    }
}
