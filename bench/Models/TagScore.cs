// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class TagScore : IGenericEquality<TagScore>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public ShallowUser user { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? score { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? post_count { get; set; }

        public bool Equals(TagScore obj)
        {
            return this.post_count.TrueEquals(obj.post_count)
                && this.score.TrueEquals(obj.score)
                && this.user.TrueEquals(obj.user);
        }
    }
}
