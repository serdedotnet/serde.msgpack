// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum InboxItemType
    {
        comment = 1,
        chat_message = 2,
        new_answer = 3,
        careers_message = 4,
        careers_invitations = 5,
        meta_question = 6,
        post_notice = 7,
        moderator_message = 8,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class InboxItem : IGenericEquality<InboxItem>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public InboxItemType? item_type { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? question_id { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? answer_id { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? comment_id { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string title { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public bool? is_unread { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public Info.Site site { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public string body { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public string link { get; set; }

        public bool Equals(InboxItem obj)
        {
            return this.answer_id.TrueEquals(obj.answer_id)
                && this.body.TrueEqualsString(obj.body)
                && this.comment_id.TrueEquals(obj.comment_id)
                && this.creation_date.TrueEquals(obj.creation_date)
                && this.is_unread.TrueEquals(obj.is_unread)
                && this.item_type.TrueEquals(obj.item_type)
                && this.link.TrueEqualsString(obj.link)
                && this.question_id.TrueEquals(obj.question_id)
                && this.site.TrueEquals(obj.site)
                && this.title.TrueEqualsString(obj.title);
        }
    }
}
