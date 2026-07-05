// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System.Collections.Generic;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class FlagOption : IGenericEquality<FlagOption>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? option_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public bool? requires_comment { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public bool? requires_site { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public bool? requires_question_id { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public string title { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public string description { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public List<FlagOption> sub_options { get; set; }

        [MessagePack.Key(7)]
        [Serde.SerdeMemberOptions(Ordinal = 7)]
        public bool? has_flagged { get; set; }

        [MessagePack.Key(8)]
        [Serde.SerdeMemberOptions(Ordinal = 8)]
        public int? count { get; set; }

        public bool Equals(FlagOption obj)
        {
            return
                this.count.TrueEquals(obj.count) &&
                this.description.TrueEqualsString(obj.description) &&
                this.has_flagged.TrueEquals(obj.has_flagged) &&
                this.option_id.TrueEquals(obj.option_id) &&
                this.requires_comment.TrueEquals(obj.requires_comment) &&
                this.requires_question_id.TrueEquals(obj.requires_question_id) &&
                this.requires_site.TrueEquals(obj.requires_site) &&
                this.sub_options.TrueEqualsList(obj.sub_options) &&
                this.title.TrueEqualsString(obj.title);
        }
    }
}
