// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Collections.Generic;

namespace Benchmark.Models
{
    [Serde.GenerateSerde(AsUnderlying = true)]
    public enum RevisionType : byte
    {
        single_user = 1,
        vote_based = 2,
    }

    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Revision : IGenericEquality<Revision>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string revision_guid { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? revision_number { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public RevisionType? revision_type { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public PostType? post_type { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? post_id { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public string comment { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public bool? is_rollback { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public string last_body { get; set; }

        [MessagePack.Key(9)]
        [Serde.SerdeMemberOptions(Ordinal = 9)]
        public string last_title { get; set; }

        [MessagePack.Key(10)]
        [Serde.SerdeMemberOptions(Ordinal = 10)]
        public List<string> last_tags { get; set; }

        [MessagePack.Key(11)]
        [Serde.SerdeMemberOptions(Ordinal = 11)]
        public string body { get; set; }

        [MessagePack.Key(12)]
        [Serde.SerdeMemberOptions(Ordinal = 12)]
        public string title { get; set; }

        [MessagePack.Key(13)]
        [Serde.SerdeMemberOptions(Ordinal = 13)]
        public List<string> tags { get; set; }

        [MessagePack.Key(14)]
        [Serde.SerdeMemberOptions(Ordinal = 14)]
        public bool? set_community_wiki { get; set; }

        [MessagePack.Key(15)]
        [Serde.SerdeMemberOptions(Ordinal = 15)]
        public ShallowUser user { get; set; }

        public bool Equals(Revision obj)
        {
            return
                this.body.TrueEqualsString(obj.body) &&
                this.comment.TrueEqualsString(obj.comment) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.is_rollback.TrueEquals(obj.is_rollback) &&
                this.last_body.TrueEqualsString(obj.last_body) &&
                this.last_tags.TrueEqualsString(obj.last_tags) &&
                this.last_title.TrueEqualsString(obj.last_title) &&
                this.post_id.TrueEquals(obj.post_id) &&
                this.post_type.TrueEquals(obj.post_type) &&
                this.revision_guid.TrueEqualsString(obj.revision_guid) &&
                this.revision_number.TrueEquals(obj.revision_number) &&
                this.revision_type.TrueEquals(obj.revision_type) &&
                this.set_community_wiki.TrueEquals(obj.set_community_wiki) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title) &&
                this.user.TrueEquals(obj.user);
        }
    }
}
