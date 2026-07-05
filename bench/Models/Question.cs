// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Question : IGenericEquality<Question>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? question_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public DateTime? last_edit_date { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public DateTime? last_activity_date { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public DateTime? locked_date { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public int? score { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public DateTime? community_owned_date { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public int? answer_count { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public int? accepted_answer_id { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public MigrationInfo migrated_to { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public MigrationInfo migrated_from { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public DateTime? bounty_closes_date { get; set; }

        [MessagePack.Key(12)]
        [Serde.SerdeMemberOptions(Ordinal = 12)]
        public int? bounty_amount { get; set; }

        [MessagePack.Key(13)]
        [Serde.SerdeMemberOptions(Ordinal = 13)]
        public DateTime? closed_date { get; set; }

        [MessagePack.Key(14)]
        [Serde.SerdeMemberOptions(Ordinal = 14)]
        public DateTime? protected_date { get; set; }

        [MessagePack.Key(15)]
        [Serde.SerdeMemberOptions(Ordinal = 15)]
        public string body { get; set; }

        [MessagePack.Key(16)]
        [Serde.SerdeMemberOptions(Ordinal = 16)]
        public string title { get; set; }

        [MessagePack.Key(17)]
        [Serde.SerdeMemberOptions(Ordinal = 17)]
        public List<string> tags { get; set; }

        [MessagePack.Key(18)]
        [Serde.SerdeMemberOptions(Ordinal = 18)]
        public string closed_reason { get; set; }

        [MessagePack.Key(19)]
        [Serde.SerdeMemberOptions(Ordinal = 19)]
        public int? up_vote_count { get; set; }

        [MessagePack.Key(20)]
        [Serde.SerdeMemberOptions(Ordinal = 20)]
        public int? down_vote_count { get; set; }

        [MessagePack.Key(21)]
        [Serde.SerdeMemberOptions(Ordinal = 21)]
        public int? favorite_count { get; set; }

        [MessagePack.Key(22)]
        [Serde.SerdeMemberOptions(Ordinal = 22)]
        public int? view_count { get; set; }

        [MessagePack.Key(23)]
        [Serde.SerdeMemberOptions(Ordinal = 23)]
        public ShallowUser owner { get; set; }

        [MessagePack.Key(24)]
        [Serde.SerdeMemberOptions(Ordinal = 24)]
        public List<Comment> comments { get; set; }

        [MessagePack.Key(25)]
        [Serde.SerdeMemberOptions(Ordinal = 25)]
        public List<Answer> answers { get; set; }

        [MessagePack.Key(26)]
        [Serde.SerdeMemberOptions(Ordinal = 26)]
        public string link { get; set; }

        [MessagePack.Key(27)]
        [Serde.SerdeMemberOptions(Ordinal = 27)]
        public bool? is_answered { get; set; }

        [MessagePack.Key(28)]
        [Serde.SerdeMemberOptions(Ordinal = 28)]
        public int? close_vote_count { get; set; }

        [MessagePack.Key(29)]
        [Serde.SerdeMemberOptions(Ordinal = 29)]
        public int? reopen_vote_count { get; set; }

        [MessagePack.Key(30)]
        [Serde.SerdeMemberOptions(Ordinal = 30)]
        public int? delete_vote_count { get; set; }

        [MessagePack.Key(31)]
        [Serde.SerdeMemberOptions(Ordinal = 31)]
        public Notice notice { get; set; }

        [MessagePack.Key(32)]
        [Serde.SerdeMemberOptions(Ordinal = 32)]
        public bool? upvoted { get; set; }

        [MessagePack.Key(33)]
        [Serde.SerdeMemberOptions(Ordinal = 33)]
        public bool? downvoted { get; set; }

        [MessagePack.Key(34)]
        [Serde.SerdeMemberOptions(Ordinal = 34)]
        public bool? favorited { get; set; }

        [MessagePack.Key(35)]
        [Serde.SerdeMemberOptions(Ordinal = 35)]
        public ShallowUser last_editor { get; set; }

        [MessagePack.Key(36)]
        [Serde.SerdeMemberOptions(Ordinal = 36)]
        public int? comment_count { get; set; }

        [MessagePack.Key(37)]
        [Serde.SerdeMemberOptions(Ordinal = 37)]
        public string body_markdown { get; set; }

        [MessagePack.Key(38)]
        [Serde.SerdeMemberOptions(Ordinal = 38)]
        public ClosedDetails closed_details { get; set; }

        [MessagePack.Key(39)]
        [Serde.SerdeMemberOptions(Ordinal = 39)]
        public string share_link { get; set; }

        public bool Equals(Question obj)
        {
            return this.accepted_answer_id.TrueEquals(obj.accepted_answer_id)
                && this.answer_count.TrueEquals(obj.answer_count)
                && this.answers.TrueEqualsList(obj.answers)
                && this.body.TrueEqualsString(obj.body)
                && this.body_markdown.TrueEqualsString(obj.body_markdown)
                && this.bounty_amount.TrueEquals(obj.bounty_amount)
                && this.bounty_closes_date.TrueEquals(obj.bounty_closes_date)
                && this.close_vote_count.TrueEquals(obj.close_vote_count)
                && this.closed_date.TrueEquals(obj.closed_date)
                && this.closed_details.TrueEquals(obj.closed_details)
                && this.closed_reason.TrueEqualsString(obj.closed_reason)
                && this.comment_count.TrueEquals(obj.comment_count)
                && this.comments.TrueEqualsList(obj.comments)
                && this.community_owned_date.TrueEquals(obj.community_owned_date)
                && this.creation_date.TrueEquals(obj.creation_date)
                && this.delete_vote_count.TrueEquals(obj.delete_vote_count)
                && this.down_vote_count.TrueEquals(obj.down_vote_count)
                && this.downvoted.TrueEquals(obj.downvoted)
                && this.favorite_count.TrueEquals(obj.favorite_count)
                && this.favorited.TrueEquals(obj.favorited)
                && this.is_answered.TrueEquals(obj.is_answered)
                && this.last_activity_date.TrueEquals(obj.last_activity_date)
                && this.last_edit_date.TrueEquals(obj.last_edit_date)
                && this.last_editor.TrueEquals(obj.last_editor)
                && this.link.TrueEqualsString(obj.link)
                && this.locked_date.TrueEquals(obj.locked_date)
                && this.migrated_from.TrueEquals(obj.migrated_from)
                && this.migrated_to.TrueEquals(obj.migrated_to)
                && this.notice.TrueEquals(obj.notice)
                && this.owner.TrueEquals(obj.owner)
                && this.protected_date.TrueEquals(obj.protected_date)
                && this.question_id.TrueEquals(obj.question_id)
                && this.reopen_vote_count.TrueEquals(obj.reopen_vote_count)
                && this.score.TrueEquals(obj.score)
                && this.share_link.TrueEqualsString(obj.share_link)
                && this.tags.TrueEqualsString(obj.tags)
                && this.title.TrueEqualsString(obj.title)
                && this.up_vote_count.TrueEquals(obj.up_vote_count)
                && this.upvoted.TrueEquals(obj.upvoted)
                && this.view_count.TrueEquals(obj.view_count);
        }

        [MessagePack.MessagePackObject]
        [Serde.GenerateSerde]
        public partial class ClosedDetails : IGenericEquality<ClosedDetails>
        {
            [MessagePack.Key(0)]
            [Serde.SerdeMemberOptions(Ordinal = 0)]
            public bool? on_hold { get; set; }

            [MessagePack.Key(1)]
            [Serde.SerdeMemberOptions(Ordinal = 1)]
            public string reason { get; set; }

            [MessagePack.Key(2)]
            [Serde.SerdeMemberOptions(Ordinal = 2)]
            public string description { get; set; }

            [MessagePack.Key(3)]
            [Serde.SerdeMemberOptions(Ordinal = 3)]
            public List<ShallowUser> by_users { get; set; }

            [MessagePack.Key(4)]
            [Serde.SerdeMemberOptions(Ordinal = 4)]
            public List<OriginalQuestion> original_questions { get; set; }

            public bool Equals(ClosedDetails obj)
            {
                return this.by_users.TrueEqualsList(obj.by_users)
                    && this.description.TrueEqualsString(obj.description)
                    && this.on_hold.TrueEquals(obj.on_hold)
                    && this.original_questions.TrueEqualsList(obj.original_questions)
                    && this.reason.TrueEqualsString(obj.reason);
            }

            [MessagePack.MessagePackObject]
            [Serde.GenerateSerde]
            public partial class OriginalQuestion : IGenericEquality<OriginalQuestion>
            {
                [MessagePack.Key(0)]
                [Serde.SerdeMemberOptions(Ordinal = 0)]
                public int? question_id { get; set; }

                [MessagePack.Key(1)]
                [Serde.SerdeMemberOptions(Ordinal = 1)]
                public string title { get; set; }

                [MessagePack.Key(2)]
                [Serde.SerdeMemberOptions(Ordinal = 2)]
                public int? answer_count { get; set; }

                [MessagePack.Key(3)]
                [Serde.SerdeMemberOptions(Ordinal = 3)]
                public int? accepted_answer_id { get; set; }

                public bool Equals(OriginalQuestion obj)
                {
                    return this.accepted_answer_id.TrueEquals(obj.accepted_answer_id)
                        && this.answer_count.TrueEquals(obj.answer_count)
                        && this.question_id.TrueEquals(obj.question_id)
                        && this.title.TrueEqualsString(obj.title);
                }
            }
        }

        [MessagePack.MessagePackObject]
        [Serde.GenerateSerde]
        public partial class Notice : IGenericEquality<Notice>
        {
            [MessagePack.Key(0)]
            [Serde.SerdeMemberOptions(Ordinal = 0)]
            public string body { get; set; }

            [MessagePack.Key(1)]
            [Serde.SerdeMemberOptions(Ordinal = 1)]
            public DateTime? creation_date { get; set; }

            [MessagePack.Key(2)]
            [Serde.SerdeMemberOptions(Ordinal = 2)]
            public int? owner_user_id { get; set; }

            public bool Equals(Notice obj)
            {
                return this.body.TrueEqualsString(obj.body)
                    && this.creation_date.TrueEquals(obj.creation_date)
                    && this.owner_user_id.TrueEquals(obj.owner_user_id);
            }
        }

        [MessagePack.MessagePackObject]
        [Serde.GenerateSerde]
        public partial class MigrationInfo : IGenericEquality<MigrationInfo>
        {
            [MessagePack.Key(0)]
            [Serde.SerdeMemberOptions(Ordinal = 0)]
            public int? question_id { get; set; }

            [MessagePack.Key(1)]
            [Serde.SerdeMemberOptions(Ordinal = 1)]
            public Info.Site other_site { get; set; }

            [MessagePack.Key(2)]
            [Serde.SerdeMemberOptions(Ordinal = 2)]
            public DateTime? on_date { get; set; }

            public bool Equals(MigrationInfo obj)
            {
                return this.on_date.TrueEquals(obj.on_date)
                    && this.other_site.TrueEquals(obj.other_site)
                    && this.question_id.TrueEquals(obj.question_id);
            }
        }
    }
}
