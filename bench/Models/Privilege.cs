// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Privilege : IGenericEquality<Privilege>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string short_description { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string description { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? reputation { get; set; }

        public bool Equals(Privilege obj)
        {
            return this.description.TrueEqualsString(obj.description)
                && this.reputation.TrueEquals(obj.reputation)
                && this.short_description.TrueEqualsString(obj.short_description);
        }
    }
}
