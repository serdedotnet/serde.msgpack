// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable IDE1006
#pragma warning disable SA1516

using System;
using System.Collections.Generic;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Answer : IGenericEquality<Answer>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? question_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? answer_id { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public DateTime? locked_date { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public DateTime? last_edit_date { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public DateTime? last_activity_date { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? score { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public DateTime? community_owned_date { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public bool? is_accepted { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public string body { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public ShallowUser owner { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public string title { get; set; }

        [MessagePack.Key(12)]
        [Serde.SerdeMemberOptions(Ordinal = 12)]
        public int? up_vote_count { get; set; }

        [MessagePack.Key(13)]
        [Serde.SerdeMemberOptions(Ordinal = 13)]
        public int? down_vote_count { get; set; }

        [MessagePack.Key(14)]
        [Serde.SerdeMemberOptions(Ordinal = 14)]
        public List<Comment> comments { get; set; }

        [MessagePack.Key(15)]
        [Serde.SerdeMemberOptions(Ordinal = 15)]
        public string link { get; set; }

        [MessagePack.Key(16)]
        [Serde.SerdeMemberOptions(Ordinal = 16)]
        public List<string> tags { get; set; }

        [MessagePack.Key(17)]
        [Serde.SerdeMemberOptions(Ordinal = 17)]
        public bool? upvoted { get; set; }

        [MessagePack.Key(18)]
        [Serde.SerdeMemberOptions(Ordinal = 18)]
        public bool? downvoted { get; set; }

        [MessagePack.Key(19)]
        [Serde.SerdeMemberOptions(Ordinal = 19)]
        public bool? accepted { get; set; }

        [MessagePack.Key(20)]
        [Serde.SerdeMemberOptions(Ordinal = 20)]
        public ShallowUser last_editor { get; set; }

        [MessagePack.Key(21)]
        [Serde.SerdeMemberOptions(Ordinal = 21)]
        public int? comment_count { get; set; }

        [MessagePack.Key(22)]
        [Serde.SerdeMemberOptions(Ordinal = 22)]
        public string body_markdown { get; set; }

        [MessagePack.Key(23)]
        [Serde.SerdeMemberOptions(Ordinal = 23)]
        public string share_link { get; set; }

        public bool Equals(Answer obj)
        {
            return
                this.accepted.TrueEquals(obj.accepted) &&
                this.answer_id.TrueEquals(obj.answer_id) &&
                this.body.TrueEqualsString(obj.body) &&
                this.body_markdown.TrueEqualsString(obj.body_markdown) &&
                this.comment_count.TrueEquals(obj.comment_count) &&
                this.comments.TrueEqualsList(obj.comments) &&
                this.community_owned_date.TrueEquals(obj.community_owned_date) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.downvoted.TrueEquals(obj.downvoted) &&
                this.is_accepted.TrueEquals(obj.is_accepted) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_edit_date.TrueEquals(obj.last_edit_date) &&
                this.last_editor.TrueEquals(obj.last_editor) &&
                this.link.TrueEqualsString(obj.link) &&
                this.locked_date.TrueEquals(obj.locked_date) &&
                this.owner.TrueEquals(obj.owner) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.score.TrueEquals(obj.score) &&
                this.share_link.TrueEqualsString(obj.share_link) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.upvoted.TrueEquals(obj.upvoted);
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Answer2 : IGenericEquality<Answer2>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? question_id { get; set; }
        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? answer_id { get; set; }
        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public DateTime? locked_date { get; set; }
        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public DateTime? creation_date { get; set; }
        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public DateTime? last_edit_date { get; set; }
        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public DateTime? last_activity_date { get; set; }
        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? score { get; set; }
        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public DateTime? community_owned_date { get; set; }
        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public bool? is_accepted { get; set; }
        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public string body { get; set; }
        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public ShallowUser2 owner { get; set; }
        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public string title { get; set; }
        [MessagePack.Key(12)]
        [Serde.SerdeMemberOptions(Ordinal = 12)]
        public int? up_vote_count { get; set; }
        [MessagePack.Key(13)]
        [Serde.SerdeMemberOptions(Ordinal = 13)]
        public int? down_vote_count { get; set; }
        [MessagePack.Key(14)]
        [Serde.SerdeMemberOptions(Ordinal = 14)]
        public List<Comment2> comments { get; set; }
        [MessagePack.Key(15)]
        [Serde.SerdeMemberOptions(Ordinal = 15)]
        public string link { get; set; }
        [MessagePack.Key(16)]
        [Serde.SerdeMemberOptions(Ordinal = 16)]
        public List<string> tags { get; set; }
        [MessagePack.Key(17)]
        [Serde.SerdeMemberOptions(Ordinal = 17)]
        public bool? upvoted { get; set; }
        [MessagePack.Key(18)]
        [Serde.SerdeMemberOptions(Ordinal = 18)]
        public bool? downvoted { get; set; }
        [MessagePack.Key(19)]
        [Serde.SerdeMemberOptions(Ordinal = 19)]
        public bool? accepted { get; set; }
        [MessagePack.Key(20)]
        [Serde.SerdeMemberOptions(Ordinal = 20)]
        public ShallowUser2 last_editor { get; set; }
        [MessagePack.Key(21)]
        [Serde.SerdeMemberOptions(Ordinal = 21)]
        public int? comment_count { get; set; }
        [MessagePack.Key(22)]
        [Serde.SerdeMemberOptions(Ordinal = 22)]
        public string body_markdown { get; set; }
        [MessagePack.Key(23)]
        [Serde.SerdeMemberOptions(Ordinal = 23)]
        public string share_link { get; set; }

        public bool Equals(Answer2 obj)
        {
            return
                this.accepted.TrueEquals(obj.accepted) &&
                this.answer_id.TrueEquals(obj.answer_id) &&
                this.body.TrueEqualsString(obj.body) &&
                this.body_markdown.TrueEqualsString(obj.body_markdown) &&
                this.comment_count.TrueEquals(obj.comment_count) &&
                this.comments.TrueEqualsList(obj.comments) &&
                this.community_owned_date.TrueEquals(obj.community_owned_date) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.downvoted.TrueEquals(obj.downvoted) &&
                this.is_accepted.TrueEquals(obj.is_accepted) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_edit_date.TrueEquals(obj.last_edit_date) &&
                this.last_editor.TrueEquals(obj.last_editor) &&
                this.link.TrueEqualsString(obj.link) &&
                this.locked_date.TrueEquals(obj.locked_date) &&
                this.owner.TrueEquals(obj.owner) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.score.TrueEquals(obj.score) &&
                this.share_link.TrueEqualsString(obj.share_link) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.upvoted.TrueEquals(obj.upvoted);
        }
    }
}
