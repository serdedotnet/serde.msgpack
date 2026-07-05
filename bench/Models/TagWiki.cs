// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class TagWiki : IGenericEquality<TagWiki>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string tag_name { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string body { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string excerpt { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public DateTime? body_last_edit_date { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public DateTime? excerpt_last_edit_date { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public ShallowUser last_body_editor { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public ShallowUser last_excerpt_editor { get; set; }

        public bool Equals(TagWiki obj)
        {
            return this.body.TrueEqualsString(obj.body)
                && this.body_last_edit_date.TrueEquals(obj.body_last_edit_date)
                && this.excerpt.TrueEqualsString(obj.excerpt)
                && this.excerpt_last_edit_date.TrueEquals(obj.excerpt_last_edit_date)
                && this.last_body_editor.TrueEquals(obj.last_body_editor)
                && this.last_excerpt_editor.TrueEquals(obj.last_excerpt_editor)
                && this.tag_name.TrueEqualsString(obj.tag_name);
        }
    }
}
