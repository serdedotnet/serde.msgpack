// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.



namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class TopTag : IGenericEquality<TopTag>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string tag_name { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? question_score { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? question_count { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? answer_score { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public int? answer_count { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public int? user_id { get; set; }

        public bool Equals(TopTag obj)
        {
            return
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.answer_score.TrueEquals(obj.answer_score) &&
                this.question_count.TrueEquals(obj.question_count) &&
                this.question_score.TrueEquals(obj.question_score) &&
                this.tag_name.TrueEqualsString(obj.tag_name) &&
                this.user_id.TrueEquals(obj.user_id);
        }
    }
}
