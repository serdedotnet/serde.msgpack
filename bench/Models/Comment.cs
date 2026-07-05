// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable IDE1006
#pragma warning disable SA1516

using System;

namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum PostType : byte
    {
        question = 1,
        answer = 2,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Comment : IGenericEquality<Comment>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? comment_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? post_id { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public PostType? post_type { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? score { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public bool? edited { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public string body { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public ShallowUser owner { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public ShallowUser reply_to_user { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public string link { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public string body_markdown { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public bool? upvoted { get; set; }

        public bool Equals(Comment obj)
        {
            return this.body.TrueEqualsString(obj.body)
                && this.body_markdown.TrueEqualsString(obj.body_markdown)
                && this.comment_id.TrueEquals(obj.comment_id)
                && this.creation_date.TrueEquals(obj.creation_date)
                && this.edited.TrueEquals(obj.edited)
                && this.link.TrueEqualsString(obj.link)
                && this.owner.TrueEquals(obj.owner)
                && this.post_id.TrueEquals(obj.post_id)
                && this.post_type.TrueEquals(obj.post_type)
                && this.reply_to_user.TrueEquals(obj.reply_to_user)
                && this.score.TrueEquals(obj.score)
                && this.upvoted.TrueEquals(obj.upvoted);
        }
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Comment2 : IGenericEquality<Comment2>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? comment_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? post_id { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public PostType? post_type { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? score { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public bool? edited { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public string body { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public ShallowUser2 owner { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public ShallowUser2 reply_to_user { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public string link { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public string body_markdown { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public bool? upvoted { get; set; }

        public bool Equals(Comment2 obj)
        {
            return this.body.TrueEqualsString(obj.body)
                && this.body_markdown.TrueEqualsString(obj.body_markdown)
                && this.comment_id.TrueEquals(obj.comment_id)
                && this.creation_date.TrueEquals(obj.creation_date)
                && this.edited.TrueEquals(obj.edited)
                && this.link.TrueEqualsString(obj.link)
                && this.owner.TrueEquals(obj.owner)
                && this.post_id.TrueEquals(obj.post_id)
                && this.post_type.TrueEquals(obj.post_type)
                && this.reply_to_user.TrueEquals(obj.reply_to_user)
                && this.score.TrueEquals(obj.score)
                && this.upvoted.TrueEquals(obj.upvoted);
        }
    }
}
